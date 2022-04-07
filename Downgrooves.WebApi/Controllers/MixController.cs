using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("mixes")]
    public class MixController : ControllerBase
    {
        private readonly ILogger<MixController> _logger;
        private readonly IMixService _service;

        public MixController(ILogger<MixController> logger, IMixService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMixes()
        {
            try
            {
                return Ok(await _service.GetMixes());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetMixes([FromQuery] PagingParameters parameters)
        {
            try
            {
                return Ok(await _service.GetMixes(parameters));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetMixesByCategory(string category)
        {
            try
            {
                return Ok(await _service.GetMixesByCategory(category));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixesByCategory {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("genre")]
        public async Task<IActionResult> GetMixesByGenre(string genre)
        {
            try
            {
                return Ok(await _service.GetMixesByGenre(genre));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixesByGenre {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/mix/{id}")]
        public async Task<IActionResult> GetMix(int id)
        {
            try
            {
                return Ok(await _service.GetMix(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}