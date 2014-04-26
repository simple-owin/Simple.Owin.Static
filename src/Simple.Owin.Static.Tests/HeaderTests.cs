namespace Simple.Owin.Static.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Xunit;

    public class HeaderTests : TestsBase
    {
        [Fact]
        public void SetsHeaderFromFileSpec()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path, "X-Test: PASS").Build()(Complete);
                app(env).Wait();

                Assert.Contains("PASS", GetHeader(env, "X-Test"));
            }
        }
        
        [Fact]
        public void SetsHeaderFromCommonSpec()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.SetCommonHeaders("X-Test: PASS").AddFile(path).Build()(Complete);
                app(env).Wait();

                Assert.Contains("PASS", GetHeader(env, "X-Test"));
            }
        }
        
        [Fact]
        public void SetsHeadersFromCommonSpecAndFileSpec()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.SetCommonHeaders("X-Common: PASS").AddFile(path, "X-File: PASS").Build()(Complete);
                app(env).Wait();

                Assert.Contains("PASS", GetHeader(env, "X-Common"));
                Assert.Contains("PASS", GetHeader(env, "X-File"));
            }
        }

        [Fact]
        public void FileHeadersOverwriteCommonHeaders()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.SetCommonHeaders("X-Common: FAIL").AddFile(path, "X-Common: PASS").Build()(Complete);
                app(env).Wait();

                Assert.Contains("PASS", GetHeader(env, "X-Common"));
            }
        }

        private static IEnumerable<string> GetHeader(IDictionary<string, object> env, string headerKey)
        {
            object obj;
            Assert.True(env.TryGetValue(OwinKeys.ResponseHeaders, out obj));

            var headers = obj as IDictionary<string, string[]>;
            Assert.NotNull(headers);

            string[] values;
            Assert.True(headers.TryGetValue(headerKey, out values));

            return values;
        }
    }
}