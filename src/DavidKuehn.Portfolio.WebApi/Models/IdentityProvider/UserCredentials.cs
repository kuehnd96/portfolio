using System.ComponentModel.DataAnnotations;
using System.Security;

namespace DavidKuehn.Portfolio.WebApi.Models.IdentityProvider
{
    public class UserCredentials
    {
        /// <summary>
        /// Gets or set the user name.
        /// </summary>
        [Required(ErrorMessage = "User name is a must")]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required(ErrorMessage = "Password is a must")]
        public String? Password { get; set; }
    }
}
