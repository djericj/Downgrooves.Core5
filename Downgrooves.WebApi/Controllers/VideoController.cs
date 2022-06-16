using Downgrooves.Model;
using Downgrooves.Service.Interfaces;
using Downgrooves.Utilities;
using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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
        public async Task<IActionResult> GetVideo(int id)
        {
            try
            {
                var video = await _service.GetVideo(id);
                return Ok(video.SetBasePath(_appConfig.CdnUrl));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetVideo)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetVideos)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetVideos)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/video")]
        public async Task<IActionResult> Add(Video video)
        {
            try
            {
                return Ok(await _service.AddVideo(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(Add)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/videos")]
        public async Task<IActionResult> AddVideos(IEnumerable<Video> videos)
        {
            try
            {
                return Ok(await _service.AddVideos(videos));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(AddVideos)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/video/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(Remove)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/videos")]
        public async Task<IActionResult> RemoveVideos([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                    await _service.Remove(id);

                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(RemoveVideos)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/video/{id}")]
        public async Task<IActionResult> UpdateVideo(Video video)
        {
            try
            {
                return Ok(await _service.UpdateVideo(video));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(UpdateVideo)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/video/{videoId}/thumbnail")]
        public async Task<IActionResult> AddThumbnail(int videoId, Thumbnail thumbnail)
        {
            try
            {
                return Ok(await _service.AddThumbnail(videoId, thumbnail));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(AddThumbnail)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/video/{videoId}/thumbnails")]
        public async Task<IActionResult> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                return Ok(await _service.AddThumbnails(videoId, thumbnails));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(AddThumbnails)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/video/{videoId}/thumbnails")]
        public async Task<IActionResult> GetThumbnails(int videoId)
        {
            try
            {
                return Ok(await _service.GetThumbnails(videoId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetThumbnails)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/video/thumbnail/{thumbnailId}")]
        public async Task<IActionResult> GetThumbnail(int thumbnailId)
        {
            try
            {
                return Ok(await _service.GetThumbnail(thumbnailId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(GetThumbnail)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/video/thumbnail/{thumbnailId}")]
        public async Task<IActionResult> RemoveThumbnail(int thumbnailId)
        {
            try
            {
                await _service.RemoveThumbnail(thumbnailId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(RemoveThumbnail)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/video/{videoId}/thumbnails")]
        public async Task<IActionResult> RemoveThumbnails(int videoId, IEnumerable<int> ids)
        {
            try
            {
                await _service.RemoveThumbnails(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(RemoveThumbnails)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/video/{videoId}/thumbnail")]
        public async Task<IActionResult> UpdateThumbnail(int videoId, Thumbnail thumbnail)
        {
            try
            {
                return Ok(await _service.UpdateThumbnail(videoId, thumbnail));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(UpdateThumbnail)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/video/{videoId}/thumbnails")]
        public async Task<IActionResult> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                return Ok(await _service.UpdateThumbnails(videoId, thumbnails));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(VideoController)}.{nameof(UpdateThumbnails)} {ex.Message} {ex.StackTrace}");
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
            return video;
        }
    }
}