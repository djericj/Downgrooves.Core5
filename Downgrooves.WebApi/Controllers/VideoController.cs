using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("videos")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _service;

        public VideoController(IVideoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            return Ok(await _service.GetVideos());
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetVideos([FromQuery] PagingParameters parameters)
        {
            return Ok(await _service.GetVideos(parameters));
        }

        [HttpPost]
        public IActionResult AddVideo(Video video)
        {
            return Ok(_service.Add(video));
        }

        [HttpPut]
        public IActionResult UpdateVideo(Video video)
        {
            return Ok(_service.Update(video));
        }
    }
}