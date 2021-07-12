using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("mixes")]
    public class MixController : ControllerBase
    {
        private readonly IMixService _service;

        public MixController(IMixService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Mix> GetMixes()
        {
            return _service.GetMixes();
        }

        [HttpGet]
        [Route("category")]
        public IEnumerable<Mix> GetMixesByCategory(string category)
        {
            return _service.GetMixesByCategory(category);
        }

        [HttpGet]
        [Route("genre")]
        public IEnumerable<Mix> GetMixesByGenre(string genre)
        {
            return _service.GetMixesByGenre(genre);
        }
    }
}