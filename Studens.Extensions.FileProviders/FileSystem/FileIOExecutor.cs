using Microsoft.Extensions.Logging;

namespace Studens.Extensions.FileProviders.FileSystem;

/// <summary>
/// File IO execution helper
/// For more info <see cref="https://docs.microsoft.com/en-us/dotnet/standard/io/handling-io-errors"/>
/// TODO: Mark as internal and init manually inside ctor?
/// </summary>
public sealed class FileIOExecutor
{
    public readonly ILogger<FileIOExecutor> _logger;
    private readonly FileProviderErrorDescriber _errorDescriber;

    public FileIOExecutor(FileProviderErrorDescriber errorDescriber, ILogger<FileIOExecutor> logger)
    {
        _errorDescriber = errorDescriber;
        _logger = logger;
    }

    /// <summary>
    /// Executes given IO operation and handles the result.
    /// </summary>
    /// <param name="ioAction">IO action</param>
    /// <param name="successResult">Result to return if operation was successful</param>
    /// <returns>File result</returns>
    public FileResult TryExecute(Action ioAction, Func<FileResult> successResult)
    {
        try
        {
            ioAction();
            return successResult();
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.FileNotFound(ex.FileName), ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.DirectoryNotFound(), ex);
        }
        catch (DriveNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.DirectoryNotFound(), ex);
        }
        catch (PathTooLongException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.PathTooLong(), ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.UnauthorizedAccess(), ex);
        }
        catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.SharingViolation(), ex);
        }
        catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 80)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.FileExists(), ex);
        }
        catch (IOException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.DefaultError());
            //Console.WriteLine($"An exception occurred:\nError code: " +
            //                  $"{ex.HResult & 0x0000FFFF}\nMessage: {ex.Message}");
        }
    }

    /// <summary>
    /// Executes given IO operation and handles the result.
    /// </summary>
    /// <param name="ioAction">IO action task</param>
    /// <param name="successResult">Result to return if operation was successful</param>
    /// <returns>File result</returns>
    public async Task<FileResult> TryExecuteAsync(Func<Task> ioAction, Func<FileResult> successResult)
    {
        try
        {
            await ioAction();
            return successResult();
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.FileNotFound(ex.FileName), ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.DirectoryNotFound(), ex);
        }
        catch (DriveNotFoundException ex)
        {
            _logger.LogError("File operation error occured", ex);
            return new FileResult(_errorDescriber.DriveNotFound(), ex);
        }
        catch (PathTooLongException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.PathTooLong(), ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.UnauthorizedAccess(), ex);
        }
        catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.SharingViolation(), ex);
        }
        catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 80)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.FileExists(), ex);
        }
        catch (IOException ex)
        {
            _logger.LogError("File operation error occured.", ex);
            return new FileResult(_errorDescriber.DefaultError());
            //Console.WriteLine($"An exception occurred:\nError code: " +
            //                  $"{ex.HResult & 0x0000FFFF}\nMessage: {ex.Message}");
        }
    }
}