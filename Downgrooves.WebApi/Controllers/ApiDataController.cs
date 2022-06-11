﻿using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("data")]
    public class ApiDataController : ControllerBase
    {
        private readonly ILogger<ApiDataController> _logger;
        private readonly IApiDataService _apiDataService;

        public ApiDataController(ILogger<ApiDataController> logger, IApiDataService apiDataService)
        {
            _apiDataService = apiDataService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetApiData(ApiData.ApiDataType dataType, string artist)
        {
            try
            {
                return Ok(await _apiDataService.GetApiData(dataType, artist));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.GetApiData {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ApiData data)
        {
            try
            {
                return Ok(await _apiDataService.Add(data));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.Add {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("reload")]
        public async Task<IActionResult> ReloadData()
        {
            try
            {
                await _apiDataService.ReloadData();
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.ReloadData {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}