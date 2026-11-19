using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _service;

        public HealthController(IOptions<AppConfig> config, ILogger<ArtistController> logger, IHealthService service)
        { 
            _service = service;
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            try
            {
                _service.GetHealth();
                return Ok(new { status = "healthy" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(503, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}
