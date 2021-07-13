using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {
        private readonly IHostEnvironment _env;

        public ConfigController(IHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("env")]
        public IActionResult GetEnvironment()
        {
            return Ok(_env.EnvironmentName);
        }
    }
}