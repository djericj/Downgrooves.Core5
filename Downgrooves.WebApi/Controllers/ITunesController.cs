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
        [Route("collection")]
        public async Task<IActionResult> AddCollection(ITunesCollection item)
        {
            try
            {
                return Ok(await _service.AddCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.AddCollection {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("collections")]
        public async Task<IActionResult> AddCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(await _service.AddCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.AddCollections {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("track")]
        public async Task<IActionResult> AddTrack(ITunesTrack item)
        {
            try
            {
                return Ok(await _service.AddTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.AddTrack {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("tracks")]
        public async Task<IActionResult> AddTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(await _service.AddTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.AddTracks {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(await _service.GetCollections(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.GetCollections {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("exclusions")]
        public async Task<IActionResult> GetExclusions()
        {
            try
            {
                return Ok(await _service.GetExclusions());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.GetExclusions {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("tracks")]
        public async Task<IActionResult> GetTracks([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(await _service.GetTracks(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.GetTracks {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("collection/{id}")]
        public async Task<IActionResult> RemoveCollection(int id)
        {
            try
            {
                await _service.RemoveCollection(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.RemoveCollection {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("track/{id}")]
        public async Task<IActionResult> RemoveTrack(int id)
        {
            try
            {
                await _service.RemoveTrack(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.RemoveTrack {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("collections")]
        public async Task<IActionResult> RemoveCollections(IEnumerable<int> ids)
        {
            try
            {
                await _service.RemoveCollections(ids);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.RemoveCollections {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("tracks")]
        public async Task<IActionResult> RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                await _service.RemoveTracks(ids);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.RemoveTracks {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collection")]
        public async Task<IActionResult> UpdateCollection(ITunesCollection item)
        {
            try
            {
                return Ok(await _service.UpdateCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.UpdateCollection {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collections")]
        public async Task<IActionResult> UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(await _service.UpdateCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.UpdateCollections {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("track")]
        public async Task<IActionResult> UpdateTrack(ITunesTrack item)
        {
            try
            {
                return Ok(await _service.UpdateTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.UpdateTrack {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("tracks")]
        public async Task<IActionResult> UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(await _service.UpdateTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.UpdateTracks {ex.Message} {ex.StackTrace}");
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}