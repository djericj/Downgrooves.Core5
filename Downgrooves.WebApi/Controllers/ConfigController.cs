using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IHostEnvironment _env;

        public ConfigController(IHostEnvironment env, ILogger<ConfigController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        [Route("env")]
        public IActionResult GetEnvironment()
        {
            return Ok(_env.EnvironmentName);
        }

        [HttpGet]
        [Route("exception")]
        public IActionResult GetTestException()
        {
            _logger.LogError("Test Exception");
            return StatusCode(500, $"Test exception generated in {_env.EnvironmentName}");
        }
    }
}