using Microsoft.AspNetCore.Identity;

namespace DavidKuehn.Portfolio.WebApi.Services.IdentityProvider
{
    /// <summary>
    /// This class is responsible for seeding the authentication system with initial roles and users.
    /// </summary>
    public static class SeedAuthentication
    {
        /// <summary>
        /// The name of the reader role. This role is used to grant read-only access to users.
        /// </summary>
        public const string ReaderRole = "Reader";
        
        /// <summary>
        /// The name of the admin role. This role is used to grant read and write access to users.
        /// </summary>
        public const string AdminRole = "Admin";
        
        /// <summary>
        /// Initializes the authentication system by creating roles and users.
        /// This method should be called at application startup to ensure that the necessary roles and users are created in the database.
        /// </summary>
        /// <param name="serviceProvider">The service provider for this API. Cannot be null.</param>
        /// <returns></returns>
        public static async Task InitializeAuthentication(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider), "Service provider cannot be null.");
            }
            
            // Retrieve the required services
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            
            // Create reader role
            var doesRoleExist = await roleManager.RoleExistsAsync(ReaderRole);
            if (!doesRoleExist)
            {
                var readerRole = new IdentityRole(ReaderRole);
                await roleManager.CreateAsync(readerRole);
            }

            // Create reader user
            var readerUser = await userManager.FindByNameAsync("reader");
            if (readerUser == null)
            {
                var newReaderUser = new IdentityUser()
                {
                  UserName = "reader"
                };

                var createdReaderUser = await userManager.CreateAsync(newReaderUser, "Sebastian2011#");
                await userManager.AddToRoleAsync(newReaderUser, ReaderRole);
            }

            // Create admin role
            doesRoleExist = await roleManager.RoleExistsAsync(AdminRole);
            if (!doesRoleExist)
            {
                var adminRole = new IdentityRole(AdminRole);
                await roleManager.CreateAsync(adminRole);
            }

            // Create admin user
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var newAdminUser = new IdentityUser()
                {
                  UserName = "admin"
                };

                var createdAdminUser = await userManager.CreateAsync(newAdminUser, "Chappy1978$!");
                await userManager.AddToRoleAsync(newAdminUser, AdminRole);
            }
        }
    }
}