namespace Simple.Owin.Static
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class SendFileShim
    {
        public static Func<string, long, long?, CancellationToken, Task> Shim(OwinEnv env)
        {
            var stream = env.ResponseBody;

#if(NET40)
            return (path, offset, count, ct) =>
            {
                var tcs = new TaskCompletionSource<int>();
                try
                {
                    using (var source = File.OpenRead(path))
                    {
                        env.ResponseHeaders["Content-Length"] = new[] { source.Length.ToString(CultureInfo.InvariantCulture) };
                        source.CopyTo(stream);
                        tcs.SetResult(0);
                    }

                }
                catch (Exception exception)
                {
                    tcs.SetException(exception);
                }

                return tcs.Task;
            };
#else
            return async (path, offset, count, ct) =>
            {
                using (var source = File.OpenRead(path))
                {
                    env.ResponseHeaders["Content-Length"] = new[] {source.Length.ToString(CultureInfo.InvariantCulture)};
                    await source.CopyToAsync(stream, 4096, ct);
                }
            };
#endif
        }
    }
}