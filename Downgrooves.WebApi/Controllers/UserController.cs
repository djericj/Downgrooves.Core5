using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(string userName, string password)
        {
            try
            {
                var user = await _service.AuthenticateAsync(userName, password);

                if (user == null)
                    return BadRequest(new { message = "Invalid username or password" });

                return Ok(user);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.UserService.Authenticate {ex.Message} {ex.StackTrace}");
                return BadRequest(new { message = $"Exception in Downgrooves.Service.UserService.Authenticate {ex.Message} {ex.StackTrace}" });
            }
        }
    }
}