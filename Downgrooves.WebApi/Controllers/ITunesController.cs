using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Downgrooves.WebApi.Controllers
{
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
        public IEnumerable<ITunesTrack> GetTracks()
        {
            return _service.GetTracks();
        }

        [HttpPost]
        public ITunesTrack AddTrack(ITunesTrack track)
        {
            return _service.Add(track);
        }

        [HttpPut]
        public ITunesTrack UpdateTrack(ITunesTrack track)
        {
            return _service.Update(track);
        }
    }
}