// ReSharper disable once CheckNamespace
namespace Simple.Owin.StaticMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class StaticBuilder
    {
        private readonly Dictionary<string, StaticFile> _files = new Dictionary<string, StaticFile>(StringComparer.InvariantCultureIgnoreCase);

        internal StaticBuilder(string path, string alias, string[] headers)
        {
            AddFileAlias(path, alias, headers);
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

        internal static IDictionary<string, string> ParseHeaders(string[] headers)
        {
            var token = new[] {':'};
            if (headers == null || headers.Length == 0)
            {
                return null;
            }

            return headers.Select(h => h.Split(token, 2)).ToDictionary(s => s[0].Trim(), s => s[1].Trim());
        }

        public Func<IDictionary<string, object>, Func<Task>, Task> Build()
        {
            return (envDict, next) =>
            {
                var env = new OwinEnv(envDict);
                var path = env.RequestPath ?? "";
                StaticFile staticFile;
                if (_files.TryGetValue(path, out staticFile))
                {
                    if (!File.Exists(staticFile.Path))
                    {
                        env.ResponseStatusCode = 404;
                        return OwinHelpers.Completed();
                    }
                    var sendFile = env.SendFileAsync ?? SendFileShim.Shim(env);
                    foreach (var header in staticFile.Headers)
                    {
                        env.ResponseHeaders[header.Key] = new[] {header.Value};
                    }
                    return sendFile(staticFile.Path, 0, null, env.CallCancelled);
                }

                return next();
            };
        }
    }
}