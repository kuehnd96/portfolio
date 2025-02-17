using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DavidKuehn.Portfolio.WebApi.Models.IdentityProvider
{
    /// <summary>
    /// Context for the security database.
    /// </summary>
    public class SecurityDbContext : IdentityDbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) 
          : base(options)
        {
        }
    }
}
