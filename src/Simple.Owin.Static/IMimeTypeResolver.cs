namespace Simple.Owin.Static
{
    /// <summary>
    /// Interface for classes that resolve MIME types from file names
    /// </summary>
    public interface IMimeTypeResolver
    {
        /// <summary>
        /// Gives the MIME type for a full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The MIME type, e.g. <c>text/html</c></returns>
        string ForFile(string path);
        /// <summary>
        /// Gives the MIME type for a full path.
        /// </summary>
        /// <param name="extension">The extension, prefixed with a period.</param>
        /// <returns>
        /// The MIME type, e.g. <c>text/html</c>
        /// </returns>
        string ForExtension(string extension);
    }
}