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
        Success = true;
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
    Error
}