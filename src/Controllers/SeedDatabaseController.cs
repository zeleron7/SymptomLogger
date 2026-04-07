using Microsoft.AspNetCore.Mvc;
using SymptomLogger.src.Services.Interfaces;

namespace SymptomLogger.src.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        #region SeedDatabase
        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            await _seedDatabaseService.SeedDatabaseAsync();
            return Ok(new { message = "Database seeded successfully" });
        }
        #endregion

        // DELETE api/SeedDatabase/clear
        #region ClearDatabase
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearDatabase()
        {
            await _seedDatabaseService.ClearDatabaseAsync();
            return Ok(new { message = "Database cleared successfully" });
        }
        #endregion 
    }
}
