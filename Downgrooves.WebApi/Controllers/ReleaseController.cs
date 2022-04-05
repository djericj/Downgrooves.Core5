using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("releases")]
    public class ReleaseController : ControllerBase
    {
        private readonly ILogger<ReleaseController> _logger;
        private readonly IReleaseService _releaseService;

        public ReleaseController(ILogger<ReleaseController> logger, IReleaseService releaseService)
        {
            _logger = logger;
            _releaseService = releaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReleases([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(await _releaseService.GetReleases(artistName));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetReleases([FromQuery] PagingParameters parameters, string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(artistName);
                releases = releases.OrderByDescending(x => x.ReleaseDate)
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize);
                return Ok(releases.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRelease(int id)
        {
            try
            {
                return Ok(await _releaseService.GetReleases(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}