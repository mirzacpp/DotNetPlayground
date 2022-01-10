namespace System.IO;

public static class StreamExtensions
{
    /// <summary>
    /// Reads all bytes from a given stream.
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <returns>Bytes array</returns>
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
    /// Reads all bytes from a given stream asynchronously.
    /// Interesting: https://stackoverflow.com/a/45462089
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Bytes array</returns>
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