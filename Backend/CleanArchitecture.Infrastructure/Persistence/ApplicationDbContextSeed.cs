using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Models;
using CleanArchitecture.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<ApplicationDbContextSeed> _logger;

        // Constructor with dependencies (recommended for DI)
        public ApplicationDbContextSeed(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ILogger<ApplicationDbContextSeed> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // Instance method 1: Seed the database
        public async Task SeedAsync()
        {
            await SeedDatabaseAsync(_context, _userManager, _roleManager, _logger);
        }

        // Instance method 2: Check and return migration status
        public async Task<bool> HasPendingMigrationsAsync()
        {
            return (await _context.Database.GetPendingMigrationsAsync()).Any();
        }
        public static async Task SeedDatabaseAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ILogger logger)
        {
            try
            {
                // Step 1: Try applying migrations
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await context.Database.MigrateAsync();
                }
                else
                {
                    logger.LogInformation("No pending migrations. Ensuring database is created...");
                    await context.Database.EnsureCreatedAsync(); // Fallback for in-memory or no migrations
                }

                // Step 2: Seed Default Roles
                await DefaultRoles.SeedAsync(userManager, roleManager);

                // Step 3: Seed Default Users
                await DefaultBasicUser.SeedAsync(userManager, roleManager);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}