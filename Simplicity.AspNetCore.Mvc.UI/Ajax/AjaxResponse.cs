namespace Simplicity.AspNetCore.Mvc.UI.Ajax;

/// <summary>
/// This class is used to create standard responses for AJAX/remote requests.
/// </summary>
[Serializable]
public class AjaxResponse : AjaxResponse<object>
{
    public AjaxResponse()
    {
    }

    public AjaxResponse(object result)
        : base(result)
    {
    }

    public AjaxResponse(bool success)
        : base(success)
    {
    }

    public AjaxResponse(ErrorInfo error)
        : base(error)
    {
    }

    public AjaxResponse(string redirectUrl)
        : base(redirectUrl)
    {
    }
}
