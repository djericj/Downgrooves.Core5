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
    [Route("mixes")]
    public class MixController : ControllerBase
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<MixController> _logger;
        private readonly IMixService _service;

        public MixController(IOptions<AppConfig> config, ILogger<MixController> logger, IMixService service)
        {
            _service = service;
            _logger = logger;
            _appConfig = config.Value;
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(Add)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(AddTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(AddTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(GetMixes)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(GetMixes)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(GetMixesByCategory)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(GetMixesByGenre)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(GetMix)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(Remove)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(RemoveTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(RemoveTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(Update)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/mix/track/{id}")]
        public async Task<IActionResult> UpdateTrack(MixTrack mixTrack)
        {
            try
            {
                return Ok(await _service.UpdateTrack(mixTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(UpdateTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(UpdateTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
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
            mix.ArtworkUrl = $"{basePath}/images/mixes/{mix.ArtworkUrl}";
            mix.AudioUrl = $"{basePath}/mp3/{mix.AudioUrl}";
            return mix;
        }
    }
}