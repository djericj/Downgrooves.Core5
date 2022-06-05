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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/releases")]
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
        public async Task<IActionResult> AddTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                return Ok(await _releaseService.AddTrack(releaseTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.AddTrack {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.AddTracks {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleases {ex.Message} {ex.StackTrace}");
                throw;
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
                var release = await _releaseService.GetReleases(x => x.Id == id);
                return Ok(release.FirstOrDefault().SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetRelease {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetReleaseTrack {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in {nameof(ReleaseController)}.GetCollection {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Remove {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Remove {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.RemoveTrack {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.RemoveTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        [Route("/release")]
        public async Task<IActionResult> Update(Release release)
        {
            try
            {
                return Ok(await _releaseService.Update(release));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        [Route("/release/track")]
        public async Task<IActionResult> UpdateTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                return Ok(await _releaseService.UpdateTrack(releaseTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.UpdateTrack {ex.Message} {ex.StackTrace}");
                throw;
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
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.UpdateTracks {ex.Message} {ex.StackTrace}");
                throw;
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
            release.BasePath = basePath;
            return release;
        }
    }
}