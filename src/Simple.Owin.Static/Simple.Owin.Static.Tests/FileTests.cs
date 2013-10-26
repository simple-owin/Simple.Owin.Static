namespace Simple.Owin.Static.Tests
{
    using Simple.Owin.Extensions;
    using Simple.Owin.Testing;
    using Xunit;

    public class FileTests
    {
        [Fact]
        public void ReturnsFile()
        {
            const string path = "/Files/index.html";

            var app = Statics.AddFile(path).Build();
            var host = new TestHostAndServer(app);
            var request = TestRequest.Get(path);
            var context = host.Process(request);

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
        
        [Fact]
        public void ReturnsAliasFile()
        {
            var app = Statics.AddFileAlias("/Files/index.html", "/").Build();
            var host = new TestHostAndServer(app);
            var request = TestRequest.Get("/");
            var context = host.Process(request);

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
    }
}
