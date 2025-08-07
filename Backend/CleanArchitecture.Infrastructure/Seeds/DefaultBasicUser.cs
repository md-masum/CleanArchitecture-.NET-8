using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //Seed admin User
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@CleanArchitecture.com",
                FirstName = "MD",
                LastName = "Masum",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (await userManager.FindByEmailAsync(adminUser.Email) is null)
            {
                await userManager.CreateAsync(adminUser, "password");
                await userManager.AddToRoleAsync(adminUser, nameof(Roles.Admin));
            }

            // Seed Customer user
            var customerUser = new ApplicationUser
            {
                UserName = "customer",
                Email = "customer@CleanArchitecture.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (await userManager.FindByEmailAsync(customerUser.Email) is null)
            {
                await userManager.CreateAsync(customerUser, "password");
                await userManager.AddToRoleAsync(customerUser, nameof(Roles.Customer));
            }

            // Seed Driver user
            var driverUser = new ApplicationUser
            {
                UserName = "driver",
                Email = "driver@CleanArchitecture.com",
                FirstName = "Jane",
                LastName = "Smith",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (await userManager.FindByEmailAsync(driverUser.Email) is null)
            {
                await userManager.CreateAsync(driverUser, "password");
                await userManager.AddToRoleAsync(driverUser, nameof(Roles.Driver));
            }
        }
    }
}
