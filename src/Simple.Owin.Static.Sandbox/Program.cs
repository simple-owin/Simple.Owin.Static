using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Owin.Static.Sandbox
{
    using Fix;

    class Program
    {
        static void Main(string[] args)
        {
            var app = new Fixer().Use(
                Statics.AddFileAlias("/index.html", "/")
                    .AddFolder("/css")
                    .AddFolderAlias("/assets/images", "/images")
                    .AddFolderAlias("/Scripts", "/js/vendor")
                ).Build();

            using (var server = Nowin.ServerBuilder.New().SetPort(8282).SetOwinApp(app).Build())
            {
                server.Start();
                Console.WriteLine("Running. Press ENTER to stop.");
                Console.ReadLine();
            }
        }
    }
}
