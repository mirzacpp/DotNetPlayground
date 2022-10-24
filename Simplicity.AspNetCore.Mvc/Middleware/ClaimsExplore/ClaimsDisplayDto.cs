namespace Simplicity.AspNetCore.Mvc.Middleware.ClaimsExplore;

/// <summary>
/// Represents a claim
/// </summary>
public class ClaimsDisplayDto
{
    public string? Issuer { get; set; }
    public string? OriginalIssuer { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
}
