using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetVideo(string id)
        {
            try
            {
                var video = await _service.GetVideo(id);
                return Ok(video.SetBasePath(_appConfig.CdnUrl));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.GetVideos {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            try
            {
                var videos = await _service.GetVideos();
                return Ok(videos.SetBasePath(_appConfig.CdnUrl));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.GetVideos {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetVideos([FromQuery] PagingParameters parameters)
        {
            try
            {
                return Ok(await _service.GetVideos(parameters));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.GetVideos {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/video")]
        public async Task<IActionResult> Add(Video video)
        {
            try
            {
                return Ok(await _service.Add(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.Add {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRange(IEnumerable<Video> videos)
        {
            try
            {
                return Ok(await _service.AddRange(videos));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.AddRange {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/video/{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.RemoveVideo {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/video/{id}")]
        public async Task<IActionResult> Update(Video video)
        {
            try
            {
                return Ok(await _service.Update(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.Update {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
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
            return video;
        }
    }
}