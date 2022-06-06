using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("mixes")]
    public class MixController : ControllerBase
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<MixController> _logger;
        private readonly IMixService _service;
        private readonly IMediaService _mediaService;

        public MixController(IOptions<AppConfig> config, ILogger<MixController> logger, IMixService service, IMediaService mediaService)
        {
            _service = service;
            _mediaService = mediaService;
            _logger = logger;
            _appConfig = config.Value;
        }

        [HttpPost]
        [Route("/mix/{id}/artwork")]
        public async Task<IActionResult> AddArtwork(int id, [FromBody] MediaFile mediaFile)
        {
            try
            {
                var path = Path.Combine(_appConfig.MediaBasePath, "images", "mixes", Path.GetFileName(mediaFile.FileName));
                var mix = await _service.GetMix(id);
                _mediaService.AddMedia(path, mediaFile);
                mix.ArtworkUrl = Path.GetFileName(mediaFile.FileName);
                var m = await _service.Update(mix);
                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddArtwork {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpDelete]
        [Route("/mix/{id}/artwork")]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            try
            {
                var mix = await _service.GetMix(id);
                var basePath = _appConfig.MediaBasePath;
                var path = Path.Combine(basePath, "images", "mixes", Path.GetFileName(mix.ArtworkUrl));
                _mediaService.RemoveMedia(path);
                mix.ArtworkUrl = "";
                var m = await _service.Update(mix);
                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddArtwork {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/mix")]
        public async Task<IActionResult> Add(Mix mix)
        {
            try
            {
                var m = await _service.Add(mix);
                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/mix/track")]
        public async Task<IActionResult> AddTrack(MixTrack mixTrack)
        {
            try
            {
                return Ok(await _service.AddTrack(mixTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        [Route("/mix/tracks")]
        public async Task<IActionResult> AddTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                return Ok(await _service.AddTracks(mixTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMixes()
        {
            try
            {
                var mixes = await _service.GetMixes();
                return Ok(mixes.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetMixes([FromQuery] PagingParameters parameters)
        {
            try
            {
                var mixes = await _service.GetMixes(parameters);
                return Ok(mixes.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetMixesByCategory(string category)
        {
            try
            {
                var mixes = await _service.GetMixesByCategory(category);
                return Ok(mixes.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixesByCategory {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("genre")]
        public async Task<IActionResult> GetMixesByGenre(string genre)
        {
            try
            {
                var mixes = await _service.GetMixesByGenre(genre);
                return Ok(mixes.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixesByGenre {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/mix/{id}")]
        public async Task<IActionResult> GetMix(int id)
        {
            try
            {
                var mix = await _service.GetMix(id);
                return Ok(mix.SetBasePath(_appConfig.CdnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.GetMixes {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("/mix/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Remove {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpDelete]
        [Route("/mix/track/{id}")]
        public async Task<IActionResult> RemoveTrack(int id)
        {
            try
            {
                await _service.RemoveTrack(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpDelete]
        [Route("/mix/tracks")]
        public async Task<IActionResult> RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                await _service.RemoveTracks(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        [Route("/mix")]
        public async Task<IActionResult> Update(Mix mix)
        {
            try
            {
                var m = await _service.Update(mix);
                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        [Route("/mix/track")]
        public async Task<IActionResult> UpdateTrack(MixTrack mixTrack)
        {
            try
            {
                return Ok(await _service.UpdateTrack(mixTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        [Route("/mix/tracks")]
        public async Task<IActionResult> UpdateTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                return Ok(await _service.UpdateTracks(mixTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }

    public static class MixControllerExtensions
    {
        public static IEnumerable<Mix> SetBasePath(this IEnumerable<Mix> mixes, string basePath)
        {
            foreach (var mix in mixes)
                mix.SetBasePath(basePath);
            return mixes;
        }

        public static Mix SetBasePath(this Mix mix, string basePath)
        {
            mix.BasePath = basePath;
            return mix;
        }
    }
}