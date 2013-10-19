namespace Simple.Owin.Static
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Simple.Owin.Helpers;

    public sealed class StaticBuilder
    {
        private readonly List<Tuple<string, string>> _commonHeaders = new List<Tuple<string, string>>(); 
        private readonly Dictionary<string, StaticFile> _files = new Dictionary<string, StaticFile>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, StaticFolder> _folders = new Dictionary<string, StaticFolder>(StringComparer.InvariantCultureIgnoreCase);
        private KeyValuePair<string, StaticFolder>[] _slowFolders;
        private Func<string, StaticFolder> _staticFolderMatcher;
        private IMimeTypeResolver _mimeTypeResolver = MimeTypeResolver.Instance;

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

        internal static StaticBuilder StartWithCommonHeaders(string[] headers)
        {
            return new StaticBuilder().SetCommonHeaders(headers);
        }

        public StaticBuilder SetCommonHeaders(params string[] headers)
        {
            _commonHeaders.AddRange(ParseHeaders(headers));
            return this;
        }

        public StaticBuilder UseMimeTypeResolver(IMimeTypeResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            _mimeTypeResolver = resolver;
            return this;
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

        internal static IList<Tuple<string, string>> ParseHeaders(string[] headers)
        {
            var token = new[] {':'};
            if (headers == null || headers.Length == 0)
            {
                return null;
            }

            return headers.Select(h => h.Split(token, 2)).Select(s => Tuple.Create(s[0].Trim(), s[1].Trim())).ToList();
        }

        public Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task> Build()
        {
            if (_files.Count == 0 && _folders.Count == 0)
            {
                return TryNext;
            }
            if (_files.Count > 0 && _folders.Count == 0)
            {
                return (envDict, next) =>
                {
                    var context = OwinContext.Get(envDict);
                    var path = context.Request.Path ?? "";
                    return TryFile(path, context) ?? TryNext(envDict, next);
                };
            }

            _staticFolderMatcher = ChooseStaticFolderMatcher();

            if (_files.Count == 0) // Then folders must have something...
            {
                return (envDict, next) =>
                {
                    var context = OwinContext.Get(envDict);
                    var path = context.Request.Path ?? "";
                    return TryFolder(path, context) ?? TryNext(envDict, next);
                };
            }

            return (envDict, next) =>
            {
                var context = OwinContext.Get(envDict);
                var path = context.Request.Path ?? "";
                return TryFile(path, context) ?? TryFolder(path, context) ?? TryNext(envDict, next);
            };
        }

        private Task TryFile(string path, OwinContext context)
        {
            StaticFile staticFile;
            if (!_files.TryGetValue(path, out staticFile)) return null;
            if (!File.Exists(staticFile.Path))
            {
                context.Response.Status = Status.Is.NotFound;
                {
                    return TaskHelper.Completed();
                }
            }
            return SendFile(context, staticFile.Path, staticFile.Headers);
        }

        private Task TryFolder(string path, OwinContext context)
        {
            var staticFolder = _staticFolderMatcher(path);

            if (staticFolder == null) return null;

            path = Path.Combine(staticFolder.Path,
                path.Substring(staticFolder.Alias.Length).Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(path)) return null;

            return SendFile(context, path, staticFolder.Headers);
        }

        private Task TryNext(IDictionary<string, object> envDict, Func<IDictionary<string, object>, Task> next)
        {
            if (next != null)
            {
                return next(envDict);
            }
            OwinContext.Get(envDict).Response.Status = Status.Is.NotFound;
            return TaskHelper.Completed();
        }

        private Task SendFile(OwinContext context, string path, IEnumerable<Tuple<string, string>> headers)
        {
            var sendFile = context.GetSendFileAsync();
            context.Response.Status = Status.Is.OK;
            context.Response.Headers.ContentType = _mimeTypeResolver.ForFile(path);

            // NOTE: The order here is important: common headers may be overwritten by item-specific headers
            foreach (var header in _commonHeaders.Concat(headers))
            {
                context.Response.Headers.SetValue(header.Item1,header.Item2);
            }
            return sendFile(path, 0, null, context.CancellationToken);
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

        public static implicit operator Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task>
            (StaticBuilder builder)
        {
            return builder.Build();
        }
    }
}