namespace Simplicity.AspNetCore.Mvc.UI.Ajax;

/// <summary>
/// Stores error informations
/// </summary>
[Serializable]
public class ErrorInfo
{
  public int Code { get; set; }
  public string Message { get; set; }
  public string Details { get; set; }

  public ErrorInfo()
  {
  }

  public ErrorInfo(int code)
  {
    Code = code;
  }

  public ErrorInfo(string message)
  {
    Message = message;
  }

  public ErrorInfo(int code, string message)
      : this(code)
  {
    Message = message;
  }

  public ErrorInfo(string message, string details)
      : this(message)
  {
    Details = details;
  }

  public ErrorInfo(int code, string message, string details)
      : this(message, details)
  {
    Code = code;
  }
}
