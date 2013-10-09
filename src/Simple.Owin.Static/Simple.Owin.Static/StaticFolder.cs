namespace Simple.Owin.Static
{
    using System.Collections.Generic;
    using System.Linq;

    internal class StaticFolder
    {
        private readonly string _path;
        private readonly string _alias;
        private readonly IDictionary<string, string> _headers;

        public StaticFolder(string path, string @alias, IDictionary<string, string> headers)
        {
            _path = MapPath.Map(path);
            _alias = alias;
            _headers = headers;
        }

        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get { return _headers ?? Enumerable.Empty<KeyValuePair<string, string>>(); }
        }

        public string Alias
        {
            get { return _alias; }
        }

        public string Path
        {
            get { return _path; }
        }
    }
}