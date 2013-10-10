namespace Simple.Owin.Static
{
    public class Statics
    {
        public static StaticBuilder AddFile(string path, params string[] headers)
        {
            return StaticBuilder.StartWithFile(path, path, headers);
        }
        
        public static StaticBuilder AddFileAlias(string path, string alias, params string[] headers)
        {
            return StaticBuilder.StartWithFile(path, alias, headers);
        }

        public static StaticBuilder AddFolder(string path, params string[] headers)
        {
            return StaticBuilder.StartWithFolder(path, path, headers);
        }

        public static StaticBuilder AddFolderAlias(string path, string alias, params string[] headers)
        {
            return StaticBuilder.StartWithFolder(path, alias, headers);
        }

        public static StaticBuilder SetCommonHeaders(params string[] headers)
        {
            return StaticBuilder.StartWithCommonHeaders(headers);
        }
    }
}
