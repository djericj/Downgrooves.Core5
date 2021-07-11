using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ITunesController : ControllerBase
    {
        private readonly ILogger<ITunesController> _logger;
        private readonly IITunesService _service;

        public ITunesController(ILogger<ITunesController> logger, IITunesService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ITunesTrack> GetTracks()
        {
            return _service.GetTracks();
        }
    }
}