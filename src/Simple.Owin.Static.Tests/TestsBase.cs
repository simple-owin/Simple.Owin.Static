namespace Simple.Owin.Static.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestsBase
    {
        protected static Dictionary<string, object> CreateEnv(string path, MemoryStream stream)
        {
            var env = new Dictionary<string, object>
            {
                {OwinKeys.CallCancelled, new CancellationToken()},
                {OwinKeys.Path, path},
                {OwinKeys.PathBase, ""},
                {OwinKeys.ResponseHeaders, new Dictionary<string, string[]>()},
                {OwinKeys.ResponseBody, stream}
            };
            return env;
        }

        protected static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        protected static Task Complete(IDictionary<string, object> _)
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(0);
            return tcs.Task;
        }

        protected static string ResponseHeader(IDictionary<string, object> env, string headerKey)
        {
            var headers = env[OwinKeys.ResponseHeaders] as IDictionary<string, string[]>;
            if (headers == null) return null;
            string[] values;
            return headers.TryGetValue(headerKey, out values) ? values.FirstOrDefault() : null;
        }
    }
}