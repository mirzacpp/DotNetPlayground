namespace Studens.Commons.Extensions;

public static class StreamExtensions
{
    public static byte[] GetAllBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Interesting: https://stackoverflow.com/a/45462089
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}