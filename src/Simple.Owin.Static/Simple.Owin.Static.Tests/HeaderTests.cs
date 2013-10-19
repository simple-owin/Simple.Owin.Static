namespace Simple.Owin.Static.Tests
{
    using Xunit;

    public class HeaderTests
    {
        [Fact]
        public void SetsHeaderFromFileSpec()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = path;

            var app = Statics.AddFile(path, "X-Test: PASS").Build();
            app(context, null).Wait();

            Assert.Equal("PASS", context.Response.Headers.GetValue("X-Test"));
        }
        
        [Fact]
        public void SetsHeaderFromCommonSpec()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = path;

            var app = Statics.SetCommonHeaders("X-Test: PASS").AddFile(path).Build();
            app(context, null).Wait();

            Assert.Equal("PASS", context.Response.Headers.GetValue("X-Test"));
        }
        
        [Fact]
        public void SetsHeadersFromCommonSpecAndFileSpec()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = path;

            var app = Statics.SetCommonHeaders("X-Common: PASS").AddFile(path, "X-File: PASS").Build();
            app(context, null).Wait();

            Assert.Equal("PASS", context.Response.Headers.GetValue("X-Common"));
            Assert.Equal("PASS", context.Response.Headers.GetValue("X-File"));
        }

        [Fact]
        public void FileHeadersOverwriteCommonHeaders()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = path;

            var app = Statics.SetCommonHeaders("X-Common: FAIL").AddFile(path, "X-Common: PASS").Build();
            app(context, null).Wait();

            Assert.Equal("PASS", context.Response.Headers.GetValue("X-Common"));
        }
    }
}