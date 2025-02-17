namespace DavidKuehn.Portfolio.WebApi.Models.IdentityProvider
{
    public class SecureResponse
    {
        public string? UserName { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string? RoleName { get; set; }
        public string? Token { get; set; }
    }
}
