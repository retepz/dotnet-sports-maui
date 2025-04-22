namespace Sports.Maui.Service;

using Sports.Maui.Service.Interface;
using System.IO.Compression;
using System.Text;

public class FileService : IFileService
{
    public async Task Compress(string path, string value)
    {
        await Compress(path, value, fromRetry: false);
    }

    public static async Task Compress(string path, string value, bool fromRetry)
    {
        try
        {
            using var fileStream = File.Create(path);
            using var gZipStream = new GZipStream(fileStream, CompressionLevel.SmallestSize, leaveOpen: false);
            await gZipStream.WriteAsync(Encoding.UTF8.GetBytes(value));
        }
        catch 
        {
            if (!fromRetry)
            {
                await Task.Delay(500);
                await Compress(path, value, fromRetry: true);
                return;
            }

            throw;
        }
    }

    public async Task<string> Decompress(string path)
    {
        using var memStream = new MemoryStream();
        using var fileStream = File.OpenRead(path);
        {
            using var gZipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            await gZipStream.CopyToAsync(memStream);
        }
    
        var array = memStream.ToArray();
        return Encoding.UTF8.GetString(array);
    }

    public void DeleteDirectoryContents(string path)
    {
        if (!Directory.Exists(path)) 
        {
            return;
        }

        var directoryInfo = new DirectoryInfo(path);
        foreach (var directory in directoryInfo.GetDirectories())
        {
            directory.Delete(true);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            file.Delete();
        }
    }
}
