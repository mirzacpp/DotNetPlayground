namespace Simplicity.Net6.WebApi.Models
{
    public class AuthenticationResultModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresAtInSeconds { get; set; }
    }
}