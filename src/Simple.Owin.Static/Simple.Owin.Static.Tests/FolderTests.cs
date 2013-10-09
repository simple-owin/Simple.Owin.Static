namespace Simple.Owin.Static.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class FolderTests
    {
        [Fact]
        public void ReturnsFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFolder("/Files").Build();
                app(env, Complete).Wait();

                stream.Position = 0;
                var text = ReadStream(stream);
                Assert.Equal("<h1>Pass</h1>", text);
            }
        }
        
        [Fact]
        public void ReturnsAliasFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFolderAlias("/Files", "/").Build();
                app(env, Complete).Wait();

                stream.Position = 0;
                var text = ReadStream(stream);
                Assert.Equal("<h1>Pass</h1>", text);
            }
        }

        private static Dictionary<string, object> CreateEnv(string path, MemoryStream stream)
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

        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static Task Complete(IDictionary<string, object> _)
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(0);
            return tcs.Task;
        }
    }
}