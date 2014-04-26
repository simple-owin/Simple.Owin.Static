namespace Simple.Owin.Static.Tests
{
    using System.IO;
    using System.Text;
    using Xunit;

    public class ContentTypeTests : TestsBase
    {
        [Fact]
        public void SetsContentTypeHeaderToTextHtmlForHtmlFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).Build()(Complete);
                app(env).Wait();

                Assert.Equal("text/html", ResponseHeader(env, "Content-Type"));
            }
        }

        [Fact]
        public void SetsCharSetForTextItem()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/index.html";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).UseCharSet(Encoding.UTF8).Build()(Complete);
                app(env).Wait();

                Assert.Equal("text/html; charset=utf-8", ResponseHeader(env, "Content-Type"));
            }
        }

        [Fact]
        public void SetsContentTypeHeaderToImagePngForPngFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/image.png";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).Build()(Complete);
                app(env).Wait();

                Assert.Equal("image/png", ResponseHeader(env, "Content-Type"));
            }
        }

        [Fact]
        public void DoesNotSetCharSetForNonTextItem()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/image.png";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).UseCharSet(Encoding.UTF8).Build()(Complete);
                app(env).Wait();

                Assert.Equal("image/png", ResponseHeader(env, "Content-Type"));
            }
        }
        
        [Fact]
        public void SetsContentTypeHeaderToApplicationJavascriptForJsFile()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/script.js";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).Build()(Complete);
                app(env).Wait();

                Assert.Equal("application/javascript", ResponseHeader(env, "Content-Type"));
            }
        }

        [Fact]
        public void SetsCharSetForJavascript()
        {
            using (var stream = new MemoryStream())
            {
                const string path = "/Files/script.js";
                var env = CreateEnv(path, stream);

                var app = Statics.AddFile(path).UseCharSet(Encoding.UTF8).Build()(Complete);
                app(env).Wait();

                Assert.Equal("application/javascript; charset=utf-8", ResponseHeader(env, "Content-Type"));
            }
        }
    }
}