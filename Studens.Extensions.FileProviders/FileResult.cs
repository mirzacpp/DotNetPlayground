using Microsoft.Extensions.FileProviders;
using Studens.Commons.Result;

namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents an file result
/// </summary>
public class FileResult : Result
{
    public FileResult(FileOperationStatus operationStatus, IFileInfo? fileInfo = null)
    {
        OperationStatus = operationStatus;
        File = fileInfo;
        Success = true;
    }

    public IFileInfo? File { get; set; }

    public FileOperationStatus OperationStatus { get; set; }

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
    Modified
}