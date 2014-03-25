namespace Simple.Owin.Static
{
    /// <summary>
    /// Static entry-point for fluent API.
    /// </summary>
    public class Statics
    {
        /// <summary>
        /// Adds a file.
        /// </summary>
        /// <param name="path">The path to the file from the web application root.</param>
        /// <param name="headers">The headers for the file.</param>
        /// <returns>Current instance.</returns>
        public static StaticBuilder AddFile(string path, params string[] headers)
        {
            return StaticBuilder.StartWithFile(path, path, headers);
        }

        /// <summary>
        /// Adds a file with an alias.
        /// </summary>
        /// <param name="path">The path to the file from the web application root.</param>
        /// <param name="alias">The public alias to use for the file.</param>
        /// <param name="headers">The headers for the file.</param>
        /// <returns>Current instance.</returns>
        public static StaticBuilder AddFileAlias(string path, string alias, params string[] headers)
        {
            return StaticBuilder.StartWithFile(path, alias, headers);
        }

        /// <summary>
        /// Adds a folder. All files below that folder will be accessible.
        /// </summary>
        /// <param name="path">The path to the folder from the web application root.</param>
        /// <param name="headers">The headers for files below the folder.</param>
        /// <returns>Current instance.</returns>
        public static StaticBuilder AddFolder(string path, params string[] headers)
        {
            return StaticBuilder.StartWithFolder(path, path, headers);
        }

        /// <summary>
        /// Adds a folder. All files below that folder will be accessible.
        /// </summary>
        /// <param name="path">The path to the folder from the web application root.</param>
        /// <param name="alias">The public alias to use for the file.</param>
        /// <param name="headers">The headers for files below the folder.</param>
        /// <returns>Current instance.</returns>
        public static StaticBuilder AddFolderAlias(string path, string alias, params string[] headers)
        {
            return StaticBuilder.StartWithFolder(path, alias, headers);
        }

        /// <summary>
        /// Sets the headers which will be added to all files served by the handler.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>Current instance.</returns>
        public static StaticBuilder SetCommonHeaders(params string[] headers)
        {
            return StaticBuilder.StartWithCommonHeaders(headers);
        }
    }
}
