using Microsoft.EntityFrameworkCore;

namespace DavidKuehn.Portfolio.WebApi.Models.Career
{
    public class CareerDbContext : DbContext
    {
        public CareerDbContext(DbContextOptions<CareerDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
