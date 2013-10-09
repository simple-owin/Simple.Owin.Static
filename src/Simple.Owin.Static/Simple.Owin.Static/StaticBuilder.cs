// ReSharper disable once CheckNamespace
namespace Simple.Owin.Static
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class StaticBuilder
    {
        private readonly Dictionary<string, StaticFile> _files = new Dictionary<string, StaticFile>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, StaticFolder> _folders = new Dictionary<string, StaticFolder>(StringComparer.InvariantCultureIgnoreCase);
        private KeyValuePair<string, StaticFolder>[] _slowFolders;
        private Func<string, StaticFolder> _staticFolderMatcher;

        private StaticBuilder()
        {
        }

        internal static StaticBuilder StartWithFile(string path, string alias, string[] headers)
        {
            return new StaticBuilder().AddFileAlias(path, alias, headers);
        }

        internal static StaticBuilder StartWithFolder(string path, string alias, string[] headers)
        {
            return new StaticBuilder().AddFolderAlias(path, alias, headers);
        }

        public StaticBuilder AddFile(string path, params string[] headers)
        {
            return AddFileAlias(path, path, headers);
        }
        
        public StaticBuilder AddFileAlias(string path, string alias, params string[] headers)
        {
            _files[alias] = new StaticFile(path, alias, ParseHeaders(headers));
            return this;
        }

        public StaticBuilder AddFolder(string path, params string[] headers)
        {
            return AddFolderAlias(path, path, headers);
        }

        public StaticBuilder AddFolderAlias(string path, string alias, params string[] headers)
        {
            if (!alias.StartsWith("/")) alias = '/' + alias;
            if (!alias.EndsWith("/")) alias = alias + '/';
            _folders[alias] = new StaticFolder(path, alias, ParseHeaders(headers));
            return this;
        }

        internal static IDictionary<string, string> ParseHeaders(string[] headers)
        {
            var token = new[] {':'};
            if (headers == null || headers.Length == 0)
            {
                return null;
            }

            return headers.Select(h => h.Split(token, 2)).ToDictionary(s => s[0].Trim(), s => s[1].Trim());
        }

        public Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task> Build()
        {
            if (_files.Count == 0 && _folders.Count == 0)
            {
                return (env, next) => next(env);
            }
            if (_files.Count > 0 && _folders.Count == 0)
            {
                return (envDict, next) =>
                {
                    var env = new OwinEnv(envDict);
                    var path = env.RequestPath ?? "";
                    return TryFile(path, env) ?? next(envDict);
                };
            }

            _staticFolderMatcher = ChooseStaticFolderMatcher();

            if (_files.Count == 0) // Then folders must have something...
            {
                return (envDict, next) =>
                {
                    var env = new OwinEnv(envDict);
                    var path = env.RequestPath ?? "";
                    return TryFolder(path, env) ?? next(envDict);
                };
            }

            return (envDict, next) =>
            {
                var env = new OwinEnv(envDict);
                var path = env.RequestPath ?? "";
                return TryFile(path, env) ?? TryFolder(path, env) ?? next(envDict);
            };
        }

        private Task TryFile(string path, OwinEnv env)
        {
            StaticFile staticFile;
            if (!_files.TryGetValue(path, out staticFile)) return null;
            if (!File.Exists(staticFile.Path))
            {
                env.ResponseStatusCode = 404;
                {
                    return OwinHelpers.Completed();
                }
            }
            return SendFile(env, staticFile.Path, staticFile.Headers);
        }

        private Task TryFolder(string path, OwinEnv env)
        {
            var staticFolder = _staticFolderMatcher(path);

            if (staticFolder == null) return null;

            path = Path.Combine(staticFolder.Path,
                path.Substring(staticFolder.Alias.Length).Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(path)) return null;

            return SendFile(env, path, staticFolder.Headers);
        }

        private static Task SendFile(OwinEnv env, string path, IEnumerable<KeyValuePair<string,string>> headers)
        {
            var sendFile = env.SendFileAsync ?? SendFileShim.Shim(env);
            foreach (var header in headers)
            {
                env.ResponseHeaders[header.Key] = new[] {header.Value};
            }
            return sendFile(path, 0, null, env.CallCancelled);
        }

        private Func<string, StaticFolder> ChooseStaticFolderMatcher()
        {
            if (_folders.Keys.Any(key => key.Count(c => c == '/') > 2))
            {
                _slowFolders = _folders.ToArray();
                return SlowStaticFolderMatcher;
            }

            return QuickStaticFolderMatcher;
        }

        private StaticFolder QuickStaticFolderMatcher(string path)
        {
            var secondSlash = path.IndexOf('/', 1);
            string search;
            switch (secondSlash)
            {
                case -1:
                    search = "/";
                    break;
                case 1:
                    return null;
                default:
                    search = path.Substring(0, secondSlash) + '/';
                    break;
            }
            StaticFolder folder;
            _folders.TryGetValue(search, out folder);
            return folder;
        }

        private StaticFolder SlowStaticFolderMatcher(string path)
        {
// ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _slowFolders.Length; i++)
            {
                if (path.StartsWith(_slowFolders[i].Key))
                {
                    return _slowFolders[i].Value;
                }
            }

            return null;
        }
    }
}