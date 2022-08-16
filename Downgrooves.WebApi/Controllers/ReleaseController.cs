using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;

using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly AppConfig _appConfig;
        private readonly ILogger<ReleaseController> _logger;
        private readonly IReleaseService _releaseService;

        public ReleaseController(IOptions<AppConfig> config, ILogger<ReleaseController> logger, IReleaseService releaseService)
        {
            _logger = logger;
            _releaseService = releaseService;
            _appConfig = config.Value;
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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(Add)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/releases")]
        public async Task<IActionResult> AddRange(IEnumerable<Release> releases)
        {
            try
            {
                foreach (var release in releases)
                    await _releaseService.Add(release);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(AddRange)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/release/track")]
        public async Task<IActionResult> AddTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                return Ok(await _releaseService.AddTrack(releaseTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(AddTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/release/tracks")]
        public async Task<IActionResult> AddTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            try
            {
                return Ok(await _releaseService.AddTracks(releaseTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(AddTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReleases([FromQuery] string artistName = null)
        {
            try
            {
                var releases = await _releaseService.GetReleases(artistName);
                return Ok(releases.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetReleases)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetReleases([FromQuery] PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false)
        {
            try
            {
                var releases = await _releaseService.GetReleases(parameters, artistName, artistId, isOriginal, isRemix);
                return Ok(releases.SetBasePath(_appConfig.CdnUrl).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetReleases)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/release/{id}")]
        public async Task<IActionResult> GetRelease(int id)
        {
            try
            {
                var release = await _releaseService.GetReleases(x => x.Id == id);
                return Ok(release.FirstOrDefault().SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetRelease)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/release/track/{id}")]
        public async Task<IActionResult> GetReleaseTrack(int id)
        {
            try
            {
                var track = await _releaseService.GetReleaseTrack(id);
                return Ok(track);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetReleaseTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/release/collection/{collectionId}")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            try
            {
                var releases = await _releaseService.GetReleases(x => x.CollectionId == collectionId);
                return Ok(releases?.FirstOrDefault().SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/release/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _releaseService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(Remove)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/releases")]
        public async Task<IActionResult> RemoveReleases(int[] ids)
        {
            try
            {
                foreach (var id in ids)
                    await _releaseService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(RemoveReleases)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/release/track/{id}")]
        public async Task<IActionResult> RemoveTrack(int id)
        {
            try
            {
                await _releaseService.RemoveTrack(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(RemoveTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/release/tracks")]
        public async Task<IActionResult> RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                await _releaseService.RemoveTracks(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(RemoveTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/release/{id}")]
        public async Task<IActionResult> Update(Release release)
        {
            try
            {
                return Ok(await _releaseService.Update(release));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(Update)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/release/track/{id}")]
        public async Task<IActionResult> UpdateTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                return Ok(await _releaseService.UpdateTrack(releaseTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(UpdateTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/release/tracks")]
        public async Task<IActionResult> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            try
            {
                return Ok(await _releaseService.UpdateTracks(releaseTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(UpdateTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }

    public static class ReleaseControllerExtensions
    {
        public static IEnumerable<Release> SetBasePath(this IEnumerable<Release> releases, string basePath)
        {
            foreach (var release in releases)
                release.SetBasePath(basePath);
            return releases;
        }

        public static Release SetBasePath(this Release release, string basePath)
        {
            if (release == null) return null;
            release.BasePath = basePath;
            release.ArtworkUrl = $"{basePath}/images/artwork/collections/{release.ArtworkUrl}";
            return release;
        }
    }
}