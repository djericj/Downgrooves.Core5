using Downgrooves.Domain.ITunes;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("itunes")]
    public class ITunesController : ControllerBase
    {
        private readonly ILogger<ITunesController> _logger;
        private readonly IITunesService _service;

        public ITunesController(ILogger<ITunesController> logger, IITunesService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ITunesLookupResultItem item)
        {
            try
            {
                return Ok(await _service.Add(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.Add {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("range")]
        public async Task<IActionResult> AddRange(IEnumerable<ITunesLookupResultItem> items)
        {
            try
            {
                return Ok(await _service.AddRange(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.Add {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                return Ok(await _service.GetItems());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.GetItems {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}