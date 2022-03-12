using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("itunes")]
    public class ITunesController : ControllerBase
    {
        private readonly IITunesService _service;

        public ITunesController(IITunesService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("tracks")]
        public IActionResult GetTracks()
        {
            return Ok(_service.GetTracks());
        }

        [HttpGet]
        [Route("tracks/paged")]
        public IActionResult GetTracks([FromQuery] PagingParameters parameters)
        {
            return Ok(_service.GetTracks(parameters));
        }

        [HttpPost]
        public IActionResult AddTrack(ITunesTrack track)
        {
            return Ok(_service.Add(track));
        }

        [HttpPut]
        public IActionResult UpdateTrack(ITunesTrack track)
        {
            return Ok(_service.Update(track));
        }
    }
}