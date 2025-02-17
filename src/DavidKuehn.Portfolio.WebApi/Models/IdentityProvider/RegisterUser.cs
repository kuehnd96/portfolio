using System.ComponentModel.DataAnnotations;

namespace DavidKuehn.Portfolio.WebApi.Models.IdentityProvider
{
    /// <summary>
    /// State of a new user.
    /// </summary>
    public class RegisterUser
    {
        // TODO: Secure strings
        
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Password is Must")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be minimum 8 characters and must be string password with uppercase character, number and sepcial character")]
        public string? Password { get; set; }
        
        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
