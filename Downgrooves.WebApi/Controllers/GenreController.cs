using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("genres")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;
        private readonly ILogger<VideoController> _logger;

        public GenreController(IGenreService service, ILogger<VideoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            try
            {
                return Ok(await _service.GetGenres());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(GenreController)}.{nameof(GetGenres)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}