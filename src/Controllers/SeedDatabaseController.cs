using Microsoft.AspNetCore.Mvc;
using SymptomLogger.src.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SymptomLogger.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDatabaseController : ControllerBase
    {
        private readonly ISeedDatabaseService _seedDatabaseService;
        private readonly ILogger<SeedDatabaseController> _logger;
        public SeedDatabaseController(ILogger<SeedDatabaseController> logger, ISeedDatabaseService seedDatabaseService)
        {
            _logger = logger;
            _seedDatabaseService = seedDatabaseService;
        }

        // POST api/SeedDatabase/seed
        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            await _seedDatabaseService.SeedDatabase();
            return Ok(new { message = "Database seeded successfully" });
        }

        // DELETE api/SeedDatabase/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> Delete()
        {
            await _seedDatabaseService.ClearDatabase();
            return Ok(new { message = "Database cleared successfully" });
        }
    }
}
