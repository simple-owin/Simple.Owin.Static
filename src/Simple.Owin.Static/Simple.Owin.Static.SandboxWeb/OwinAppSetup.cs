namespace Simple.Owin.Static.SandboxWeb
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OwinAppSetup
    {
        public void Setup(
            Action<Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task>> use)
        {
            use(Statics.AddFileAlias("/index.html", "/")
                .AddFolder("/css")
                .AddFolderAlias("/assets/images", "/images")
                .AddFolderAlias("/Scripts", "/js/vendor"));
        }
    }
}