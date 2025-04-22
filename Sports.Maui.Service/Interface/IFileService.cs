namespace Sports.Maui.Service.Interface;

public interface IFileService
{
    Task Compress(string path, string value);
    Task<string> Decompress(string path);
    void DeleteDirectoryContents(string path);
}
