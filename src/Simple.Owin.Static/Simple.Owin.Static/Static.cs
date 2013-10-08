using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Simple.Owin
{
    using StaticMiddleware;

    public class Static
    {
        public static StaticBuilder AddFile(string path, params string[] headers)
        {
            return new StaticBuilder(path, path, headers);
        }
        
        public static StaticBuilder AddFileAlias(string path, string alias, params string[] headers)
        {
            return new StaticBuilder(path, alias, headers);
        }
    }
}
