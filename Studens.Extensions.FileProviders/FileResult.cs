using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents an file result
/// </summary>
public class FileResult
{
    public FileResult(FileOperationStatus operationStatus, IFileInfo? fileInfo = null)
    {
        OperationStatus = operationStatus;
        File = fileInfo;
        Success = operationStatus != FileOperationStatus.Error && operationStatus != FileOperationStatus.NotFound;
    }

    public FileResult(FileProviderError error, Exception? exception = null)
    {
        Error = error;
        Exception = exception;
        Success = false;
        OperationStatus = FileOperationStatus.Error;
    }

    public bool Success { get; }

    public IFileInfo? File { get; }

    public FileOperationStatus OperationStatus { get; }

    /// <summary>
    /// Error that occured in file operation
    /// </summary>
    public FileProviderError? Error { get; }

    /// <summary>
    /// Exception that occured in file operation
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Returns file deleted operation result
    /// </summary>
    public static FileResult FileDeleteResult() => new(FileOperationStatus.Deleted);

    /// <summary>
    /// Returns file created operation result
    /// </summary>
    public static FileResult FileCreatedResult(IFileInfo fileInfo) => new(FileOperationStatus.Created, fileInfo);

    /// <summary>
    /// Returns file modified operation result
    /// </summary>
    public static FileResult FileModifiedResult(IFileInfo fileInfo) => new(FileOperationStatus.Modified, fileInfo);

    /// <summary>
    /// Returns file unmodified operation result
    /// </summary>
    public static FileResult FileUnmodifiedResult(IFileInfo fileInfo) => new(FileOperationStatus.Unmodified, fileInfo);

    /// <summary>
    /// Returns file not found result
    /// </summary>
    /// <param name="filePath">File path</param>
    public static FileResult FileNotFoundResult(string filePath) => new(FileOperationStatus.NotFound, new NotFoundFileInfo(filePath));
}

/// <summary>
/// Represents an generic file result
/// </summary>
/// <typeparam name="TFileInfo">Type of model to return</typeparam>
public class FileResult<TFileInfo> where TFileInfo : IFileInfo
{
    public FileResult(FileOperationStatus operationStatus, TFileInfo fileInfo)
    {
        OperationStatus = operationStatus;
        File = fileInfo;
        Success = operationStatus != FileOperationStatus.Error && operationStatus != FileOperationStatus.NotFound;
    }

    public FileResult(FileProviderError error, Exception? exception = null)
    {
        Error = error;
        Exception = exception;
        Success = false;
        OperationStatus = FileOperationStatus.Error;
    }

    public bool Success { get; }

    public TFileInfo File { get; }

    public FileOperationStatus OperationStatus { get; }

    /// <summary>
    /// Error that occured in file operation
    /// </summary>
    public FileProviderError? Error { get; }

    /// <summary>
    /// Exception that occured in file operation
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Returns file deleted operation result
    /// </summary>
    public static FileResult FileDeleteResult() => new(FileOperationStatus.Deleted);

    /// <summary>
    /// Returns file created operation result
    /// </summary>
    public static FileResult FileCreatedResult(TFileInfo fileInfo) => new(FileOperationStatus.Created, fileInfo);

    /// <summary>
    /// Returns file modified operation result
    /// </summary>
    public static FileResult FileModifiedResult(TFileInfo fileInfo) => new(FileOperationStatus.Modified, fileInfo);

    /// <summary>
    /// Returns file unmodified operation result
    /// </summary>
    public static FileResult FileUnmodifiedResult(TFileInfo fileInfo) => new(FileOperationStatus.Unmodified, fileInfo);

    /// <summary>
    /// Returns file not found result
    /// </summary>
    /// <param name="filePath">File path</param>
    public static FileResult FileNotFoundResult(string filePath) => new(FileOperationStatus.NotFound, new NotFoundFileInfo(filePath));
}

/// <summary>
/// Represents file operations status
/// </summary>
public enum FileOperationStatus
{
    /// <summary>
    /// File created
    /// </summary>
    Created,

    /// <summary>
    /// File deleted
    /// </summary>
    Deleted,

    /// <summary>
    /// File unmodified(Can occur if file that is being created already exists)
    /// </summary>
    Unmodified,

    /// <summary>
    /// File modified(File updated)
    /// </summary>
    Modified,

    /// <summary>
    /// Operation resulted with error
    /// </summary>
    Error,

    /// <summary>
    /// File not found
    /// </summary>
    NotFound
}