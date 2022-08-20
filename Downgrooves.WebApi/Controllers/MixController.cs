using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

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
        public IActionResult Add(Mix mix)
        {
            try
            {
                var m = _service.Add(mix);
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
        public IActionResult AddTrack(MixTrack mixTrack)
        {
            try
            {
                return Ok(_service.AddTrack(mixTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(AddTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("/mix/tracks")]
        public IActionResult AddTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                return Ok(_service.AddTracks(mixTracks));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(AddTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        public IActionResult GetMixes()
        {
            try
            {
                var mixes = _service.GetMixes();
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
        public IActionResult GetMixes([FromQuery] PagingParameters parameters)
        {
            try
            {
                var mixes = _service.GetMixes(parameters);
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
        public IActionResult GetMixesByCategory(string category)
        {
            try
            {
                var mixes = _service.GetMixesByCategory(category);
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
        public IActionResult GetMixesByGenre(string genre)
        {
            try
            {
                var mixes = _service.GetMixesByGenre(genre);
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
        public IActionResult GetMix(int id)
        {
            try
            {
                var mix = _service.GetMix(id);
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
        public IActionResult Remove(int id)
        {
            try
            {
                _service.Remove(id);
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
        public IActionResult RemoveTrack(int id)
        {
            try
            {
                _service.RemoveTrack(id);
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
        public IActionResult RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                _service.RemoveTracks(ids);
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
        public IActionResult Update(Mix mix)
        {
            try
            {
                var m = _service.Update(mix);
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
        public IActionResult UpdateTrack(MixTrack mixTrack)
        {
            try
            {
                return Ok(_service.UpdateTrack(mixTrack));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(MixController)}.{nameof(UpdateTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("/mix/tracks")]
        public IActionResult UpdateTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                return Ok(_service.UpdateTracks(mixTracks));
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