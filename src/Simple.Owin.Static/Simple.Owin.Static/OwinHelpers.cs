namespace Simple.Owin.StaticMiddleware
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal static class OwinHelpers
    {
        public static void SetResponseHeaderValue(IDictionary<string, object> env, string key, string value)
        {
            var headers = env.GetValueOrDefault<IDictionary<string, string[]>>(OwinKeys.ResponseHeaders);
            if (headers == null)
            {
                headers = new Dictionary<string, string[]>();
                env[OwinKeys.ResponseHeaders] = headers;
            }

            headers[key] = new[] { value };
        }

        public static T GetValueOrDefault<T>(this IDictionary<string, object> env, string key)
        {
            object obj;
            if (env.TryGetValue(key, out obj))
            {
                if (obj is T)
                {
                    return (T)obj;
                }
            }

            return default(T);
        }

        public static Task Completed()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(0);
            return tcs.Task;
        }
    }
}