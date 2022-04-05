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
        [Route("range")]
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

        [HttpGet]
        [Route("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(artistName);
                return Ok(releases.Where(x => x.WrapperType == "collection"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetCollections {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("collections/paged")]
        public async Task<IActionResult> GetCollections([FromQuery] PagingParameters parameters, string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(artistName);
                releases = releases.OrderByDescending(x => x.ReleaseDate)
                    .Where(x => x.WrapperType == "collection")
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize);
                return Ok(releases.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetCollections {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("exclusions")]
        public IActionResult GetExclusions() => Ok(_releaseService.GetExclusions());

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

        [HttpGet]
        [Route("collection/{collectionId}")]
        public async Task<IActionResult> GetReleaseByCollectionId(int collectionId)
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

        [HttpGet]
        [Route("track/{trackId}")]
        public async Task<IActionResult> GetReleaseByTrackId(int trackId)
        {
            try
            {
                return Ok(await _releaseService.GetReleases(x => x.TrackId == trackId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
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
                return Ok(await _releaseService.GetReleases(parameters, artistName));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("tracks")]
        public async Task<IActionResult> GetTracks([FromQuery] string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(x => x.WrapperType == "track");
                if (artistName != null)
                    releases = releases.Where(x => x.TrackName.Contains(artistName));
                return Ok(releases.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        [Route("tracks/paged")]
        public async Task<IActionResult> GetTracks([FromQuery] PagingParameters parameters, string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(artistName);
                releases = releases.OrderByDescending(x => x.ReleaseDate)
                    .Where(x => x.WrapperType == "track")
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize);
                return Ok(releases.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpDelete]
        public void Remove([FromQuery] int Id)
        {
            try
            {
                _releaseService.Remove(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.Remove {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Release release)
        {
            try
            {
                return Ok(await _releaseService.Update(release));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}