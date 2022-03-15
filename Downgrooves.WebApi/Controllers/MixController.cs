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
            return Ok(await _service.GetShowMixes());
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetMixes([FromQuery] PagingParameters parameters)
        {
            return Ok(await _service.GetShowMixes(parameters));
        }

        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetMixesByCategory(string category)
        {
            return Ok(await _service.GetMixesByCategory(category));
        }

        [HttpGet]
        [Route("genre")]
        public async Task<IActionResult> GetMixesByGenre(string genre)
        {
            return Ok(await _service.GetMixesByGenre(genre));
        }

        [HttpPost]
        public async Task<IActionResult> Add(Mix mix)
        {
            return Ok(await _service.Add(mix));
        }
    }
}