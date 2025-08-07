using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@CleanArchitecture.com",
                FirstName = "MD",
                LastName = "MASUM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user is null)
            {
                await userManager.CreateAsync(defaultUser, "password");
                await userManager.AddToRoleAsync(defaultUser, nameof(Roles.Admin));
                await userManager.AddToRoleAsync(defaultUser, nameof(Roles.Driver));
                await userManager.AddToRoleAsync(defaultUser, nameof(Roles.Customer));
            }
        }
    }
}
