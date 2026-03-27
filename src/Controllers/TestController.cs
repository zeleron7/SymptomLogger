using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SymptomLogger.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("Exception in HomeController.");
        }

    }
}
