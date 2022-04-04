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
    [Route("videos")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _service;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IVideoService service, ILogger<VideoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            try
            {
                return Ok(await _service.GetVideos());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.GetVideos {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetVideos([FromQuery] PagingParameters parameters)
        {
            try
            {
                return Ok(await _service.GetVideos(parameters));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.GetVideos {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        public IActionResult Add(Video video)
        {
            try
            {
                return Ok(_service.Add(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.Add {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        public IActionResult Update(Video video)
        {
            try
            {
                return Ok(_service.Update(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.Update {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}