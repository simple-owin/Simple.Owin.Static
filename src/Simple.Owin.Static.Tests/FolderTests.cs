namespace Simple.Owin.Static.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class FolderTests : TestsBase
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
    }
}