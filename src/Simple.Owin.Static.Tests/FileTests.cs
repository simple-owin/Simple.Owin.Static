namespace Simple.Owin.Static.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Static;
    using Xunit;

    public class FileTests : TestsBase
    {
        [Fact]
        public void ReturnsFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).Build()(Complete);
                app(env).Wait();

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
                const string path = "/Files/index.html";
                var env = CreateEnv("/", stream);

                var app = Statics.AddFileAlias(path, "/").Build()(Complete);
                app(env).Wait();

                stream.Position = 0;
                var text = ReadStream(stream);
                Assert.Equal("<h1>Pass</h1>", text);
            }
        }

        [Fact]
        public void ReturnsAliasFileWithTrailingSlash()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv("/foo/", stream);

                var app = Statics.AddFileAlias(path, "/foo").Build()(Complete);
                app(env).Wait();

                stream.Position = 0;
                var text = ReadStream(stream);
                Assert.Equal("<h1>Pass</h1>", text);
            }
        }
    }
}
