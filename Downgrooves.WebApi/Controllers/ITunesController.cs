using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetTracks()
        {
            return Ok(await _service.GetTracks());
        }

        [HttpGet]
        [Route("tracks/paged")]
        public async Task<IActionResult> GetTracksAsync([FromQuery] PagingParameters parameters)
        {
            return Ok(await _service.GetTracks(parameters));
        }

        [HttpPost]
        public async Task<IActionResult> AddTrack(ITunesTrack track)
        {
            return Ok(await _service.Add(track));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTrack(ITunesTrack track)
        {
            return Ok(await _service.Update(track));
        }
    }
}