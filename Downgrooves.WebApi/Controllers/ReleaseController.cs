using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("releases")]
    public class ReleaseController : ControllerBase
    {
        private readonly IReleaseService _service;

        public ReleaseController(IReleaseService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetReleases()
        {
            return Ok(await _service.GetReleases());
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetReleasesAsync([FromQuery] PagingParameters parameters)
        {
            return Ok(await _service.GetReleases(parameters));
        }
    }
}
