using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Route("/release")]
        public async Task<IActionResult> Add(Release release)
        {
            try
            {
                return Ok(await _releaseService.Add(release));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRange(IEnumerable<Release> releases)
        {
            try
            {
                return Ok(await _releaseService.AddRange(releases));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/release/track")]
        public async Task<IActionResult> Add(ReleaseTrack releaseTrack)
        {
            try
            {
                return Ok(await _releaseService.Add(releaseTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/release/tracks")]
        public async Task<IActionResult> AddRange(IEnumerable<ReleaseTrack> releaseTracks)
        {
            try
            {
                return Ok(await _releaseService.AddRange(releaseTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
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
        [Route("/release/{id}")]
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

        [HttpGet]
        [Route("/release/collection/{collectionId}")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            try
            {
                return Ok(await _releaseService.GetReleases(x => x.CollectionId == collectionId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}