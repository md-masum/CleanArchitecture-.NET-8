using CleanArchitecture.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController(IMediator mediator, ApplicationDbContextSeed dbSeeder) : BaseApiController(mediator)
    {
        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return Ok("Pong");
        }

        [HttpGet("status")]
        public ActionResult<string> Status()
        {
            return Ok("System is running");
        }

        [HttpPost("database-migration-status")]
        public async Task<ActionResult<string>> DatabaseMigrationStatus()
        {
            if (await dbSeeder.HasPendingMigrationsAsync())
            {
                return BadRequest("Database has pending migrations. Please apply them before proceeding.");
            }

            return Ok("Database is up to date with no pending migrations.");
        }

        [HttpPost("seed-database")]
        public async Task<ActionResult<string>> SeedDatabase()
        {
            try
            {
                // Seed the database
                await dbSeeder.SeedAsync();
                return Ok("Database seeded successfully.");
            }
            catch (Exception e)
            {
                return BadRequest($"Error seeding database: {e.Message}");
            }
        }
    }
}
