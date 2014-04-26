using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Owin.Static.SandboxWeb
{
    using AppFunc = Func<IDictionary<string,object>,Task>;
    public class OwinAppSetup
    {
        public void Setup(Action<Func<AppFunc,AppFunc>> use)
        {
            use(Statics.AddFileAlias("/index.html", "/")
                .AddFolder("/css")
                .AddFolderAlias("/assets/images", "/images")
                .AddFolderAlias("/Scripts", "/js/vendor")
                .UseCharSet(Encoding.UTF8));
        }
    }
}