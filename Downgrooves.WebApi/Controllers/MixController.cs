using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
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
        public async Task<IActionResult> Add(Mix mix)
        {
            try
            {
                return Ok(await _service.Add(mix));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Mix mix)
        {
            try
            {
                return Ok(await _service.Update(mix));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
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