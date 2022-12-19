namespace Simplicity.AspNetCore.Mvc.UI.Ajax;

public abstract class AjaxResponseBase
{
  public string RedirectUrl { get; set; }

  public bool Success { get; set; }

  public ErrorInfo Error { get; set; }
}
