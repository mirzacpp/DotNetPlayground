namespace Simplicity.AspNetCore.Mvc.UI.Ajax;

[Serializable]
public class AjaxResponse<TResult> : AjaxResponseBase
{
  public TResult Result { get; set; }

  public AjaxResponse(TResult result)
  {
    Result = result;
    Success = true;
  }

  public AjaxResponse(string redirectUrl)
      : this()
  {
    RedirectUrl = redirectUrl;
  }

  public AjaxResponse()
  {
    Success = true;
  }

  public AjaxResponse(bool success)
  {
    Success = success;
  }

  public AjaxResponse(ErrorInfo error)
  {
    Error = error;
    Success = false;
  }
}
