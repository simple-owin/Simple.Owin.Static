namespace Simple.Owin.Static.Tests
{
    using Simple.Owin.Extensions;
    using Static;
    using Xunit;

    public class FileTests
    {
        [Fact]
        public void ReturnsFile()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = path;

            var app = Statics.AddFile(path).Build();
            app(context, null).Wait();

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
        
        [Fact]
        public void ReturnsAliasFile()
        {
            const string path = "/Files/index.html";
            var context = OwinContext.Create();
            context.Request.Path = "/";

            var app = Statics.AddFileAlias(path, "/").Build();
            app(context, null).Wait();

            context.Response.Body.Position = 0;
            var text = context.Response.Body.ReadAll();
            Assert.Equal("<h1>Pass</h1>", text);
        }
    }
}
