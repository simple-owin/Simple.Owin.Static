namespace Simple.Owin.StaticMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class SendFileShim
    {
        public static Func<string, long, long?, CancellationToken, Task> Shim(OwinEnv env)
        {
            var stream = env.ResponseBody;

            return async (path, offset, count, ct) =>
            {
                using (var source = File.OpenRead(path))
                {
                    env.ResponseHeaders["Content-Length"] = new[] {source.Length.ToString(CultureInfo.InvariantCulture)};
                    await source.CopyToAsync(stream, 4096, ct);
                }
            };
        }
    }
}