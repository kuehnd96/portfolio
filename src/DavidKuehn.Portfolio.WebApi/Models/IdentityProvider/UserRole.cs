namespace DavidKuehn.Portfolio.WebApi.Models.IdentityProvider
{
    /// <summary>
    /// State of a role assigned to a user.
    /// </summary>
    public class UserRole
    {
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
    }
}
