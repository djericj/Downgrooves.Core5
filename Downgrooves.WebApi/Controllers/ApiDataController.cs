using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/data")]
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
        public IActionResult GetApiData()
        {
            try
            {
                return Ok(_apiDataService.GetApiData());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ApiDataController)}.{nameof(GetApiData)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/data/{dataType}")]
        public IActionResult GetApiData(ApiData.ApiDataTypes dataType)
        {
            try
            {
                return Ok(_apiDataService.GetApiData(dataType));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ApiDataController)}.{nameof(GetApiData)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("/data/{dataType}/artist/{artist}")]
        public IActionResult GetApiData(ApiData.ApiDataTypes dataType, string artist)
        {
            try
            {
                return Ok(_apiDataService.GetApiData(dataType, artist));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ApiDataController)}.{nameof(GetApiData)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] ApiData data)
        {
            try
            {
                return Ok(_apiDataService.Add(data));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ApiDataController)}.{nameof(Add)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        public IActionResult Update(ApiData data)
        {
            try
            {
                return Ok(_apiDataService.Update(data));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ApiDataController)}.{nameof(Update)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}