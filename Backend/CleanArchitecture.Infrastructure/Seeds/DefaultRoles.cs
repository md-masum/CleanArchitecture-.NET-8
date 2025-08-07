using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //Seed Roles
            var isAdminExist = await roleManager.FindByNameAsync(nameof(Roles.Admin));
            if (isAdminExist is null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(nameof(Roles.Admin)));
            }

            var isDriverExist = await roleManager.FindByNameAsync(nameof(Roles.Driver));
            if (isDriverExist is null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(nameof(Roles.Driver)));
            }

            var isCustomerExist = await roleManager.FindByNameAsync(nameof(Roles.Customer));
            if (isCustomerExist is null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(nameof(Roles.Customer)));
            }
        }
    }
}
