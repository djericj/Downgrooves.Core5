using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("videos")]
    public class VideoController : ControllerBase
    {
        private readonly AppConfig _appConfig;
        private readonly IVideoService _service;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IOptions<AppConfig> config, IVideoService service, ILogger<VideoController> logger)
        {
            _service = service;
            _logger = logger;
            _appConfig = config.Value;
        }

        [HttpGet]
        [Route("/video/{id}")]
        public IActionResult GetVideo(int id)
        {
            try
            {
                var video = _service.GetVideo(id);
                return Ok(video.SetBasePath(_appConfig.CdnUrl));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetVideo)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                var videos = _service.GetVideos();
                return Ok(videos.SetBasePath(_appConfig.CdnUrl));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetVideos)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }

    public static class VideoControllerExtensions
    {
        public static IEnumerable<Video> SetBasePath(this IEnumerable<Video> videos, string basePath)
        {
            foreach (var video in videos)
                video.SetBasePath(basePath);
            return videos;
        }

        public static Video SetBasePath(this Video video, string basePath)
        {
            video.BasePath = basePath;
            video.ArtworkUrl = $"{basePath}/images/artwork/videos/{video.SourceSystemId}/high.jpg";
            video.VideoUrl = $"https://youtu.be/{video.SourceSystemId}";
            return video;
        }
    }
}