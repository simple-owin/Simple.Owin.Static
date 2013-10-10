namespace Simple.Owin.Static
{
    public interface IMimeTypeResolver
    {
        string ForFile(string path);
        string ForExtension(string extension);
    }
}