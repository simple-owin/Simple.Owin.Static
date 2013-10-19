using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Simple.Owin.Extensions;

namespace Simple.Owin.Static
{
    using SendFile = Func<string, long, long?, CancellationToken, Task>;

    internal static class OwinContextExtensions
    {
        public const string SendFileAsync = "sendfile.SendAsync";
        //todo move to response extensions?

        public static SendFile GetSendFileAsync(this IContext context)
        {
            return context.Environment.GetValueOrCreate<SendFile>(SendFileAsync,
                () =>
                {
                    var stream = context.Response.Body;

                    return async (path, offset, count, ct) =>
                    {
                        using (var source = File.OpenRead(path))
                        {
                            context.Response.Headers.ContentLength = source.Length;
                            await source.CopyToAsync(stream, 4096, ct);
                        }
                    };
                });
        }
    }
}