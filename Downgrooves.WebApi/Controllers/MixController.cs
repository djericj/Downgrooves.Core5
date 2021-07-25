using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetMixes()
        {
            return Ok(await _service.GetShowMixesAsync());
        }

        [HttpGet]
        [Route("category")]
        public IActionResult GetMixesByCategory(string category)
        {
            return Ok(_service.GetMixesByCategory(category));
        }

        [HttpGet]
        [Route("genre")]
        public IActionResult GetMixesByGenre(string genre)
        {
            return Ok(_service.GetMixesByGenre(genre));
        }

        [HttpPost]
        public IActionResult Add(Mix mix)
        {
            return Ok(_service.Add(mix));
        }
    }
}