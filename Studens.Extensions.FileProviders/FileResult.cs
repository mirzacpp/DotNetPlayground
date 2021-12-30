using Microsoft.Extensions.FileProviders;
using Studens.Commons.Result;

namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents an file result
/// </summary>
public class FileResult : Result
{
    public IFileInfo? File { get; set; }
}
