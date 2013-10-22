namespace Simple.Owin.Static.Tests
{
    using Simple.Owin.Extensions;
    using Simple.Owin.Testing;
    using Xunit;

    public class FolderTests
    {
        [Fact]
        public void ReturnsFile()
        {
            var app = Statics.AddFolder("/Files").Build();
            var host = new TestHostAndServer(app);
            var request = TestRequest.Get("/Files/index.html");
            var context = host.Process(request);

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
        
        [Fact]
        public void ReturnsAliasFile()
        {
            var app = Statics.AddFolderAlias("/Files", "/").Build();
            var host = new TestHostAndServer(app);
            var request = TestRequest.Get("/index.html");
            var context = host.Process(request);

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
    }
}