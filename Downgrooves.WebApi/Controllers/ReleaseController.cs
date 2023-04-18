using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        [HttpGet("/releases/{artistName?}")]
        public IActionResult GetReleases(string artistName)
        {
            try
            {
                var releases = _releaseService.GetAll(artistName).OrderByDescending(r => r.ReleaseDate).ToList();
                return Ok(releases.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetReleases)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet("/release/{id}")]
        public IActionResult GetRelease(int id)
        {
            try
            {
                var release = _releaseService.Get(id);
                return Ok(release.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ReleaseController)}.{nameof(GetRelease)} {ex.Message} {ex.StackTrace}");
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
            release.ArtworkUrl = $"{basePath}/images/artwork/{release.ArtworkUrl}";
            return release;
        }
    }
}