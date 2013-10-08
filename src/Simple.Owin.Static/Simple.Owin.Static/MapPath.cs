namespace Simple.Owin.StaticMiddleware
{
    using System;
    using System.IO;
    using System.Reflection;

    internal static class MapPath
    {
        private static Func<string, string> _mapPath;

        public static Func<string, string> Map
        {
            get { return _mapPath ?? (_mapPath = Load()); }
        }

        private static Func<string, string> Load()
        {
            var hostingEnvironment = Type.GetType("System.Web.Hosting.HostingEnvironment, System.Web");
            if (hostingEnvironment != null)
            {
                var func =
                    (Func<string, string>)
                        hostingEnvironment.GetMethod("MapPath").CreateDelegate(typeof (Func<string, string>));

                return path => func(path) ?? FallbackMapPath(path);
            }

            return FallbackMapPath;
        }

        private static string FallbackMapPath(string virtualPath)
        {
            var assembly = Assembly.GetEntryAssembly() ??
                           Assembly.GetCallingAssembly();
            var path = Path.GetDirectoryName(assembly.GetPath());

            if (path == null)
                throw new Exception("Unable to determine executing assembly path.");

            return Path.Combine(path, virtualPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        }

        private static string GetPath(this Assembly assembly)
        {
            return new Uri(assembly.EscapedCodeBase).LocalPath;
        }
    }
}