using Downgrooves.Model.ITunes;
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
        private readonly IITunesService _itunesService;

        public ITunesController(ILogger<ITunesController> logger, IITunesService itunesService)
        {
            _itunesService = itunesService;
            _logger = logger;
        }

        [HttpPost]
        [Route("collection")]
        public async Task<IActionResult> AddCollection(ITunesCollection item)
        {
            try
            {
                return Ok(await _itunesService.AddCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("collections")]
        public async Task<IActionResult> AddCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(await _itunesService.AddCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("track")]
        public async Task<IActionResult> AddTrack(ITunesTrack item)
        {
            try
            {
                return Ok(await _itunesService.AddTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("tracks")]
        public async Task<IActionResult> AddTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(await _itunesService.AddTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(await _itunesService.GetCollections(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("collection/{id}")]
        public async Task<IActionResult> GetCollection(int id)
        {
            try
            {
                return Ok(await _itunesService.GetCollection(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("exclusions")]
        public async Task<IActionResult> GetExclusions()
        {
            try
            {
                return Ok(await _itunesService.GetExclusions());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetExclusions)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("tracks")]
        public async Task<IActionResult> GetTracks([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(await _itunesService.GetTracks(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("track/{id}")]
        public async Task<IActionResult> GetTrack(int id)
        {
            try
            {
                return Ok(await _itunesService.GetTrack(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("collection/{id}")]
        public async Task<IActionResult> RemoveCollection(int id)
        {
            try
            {
                await _itunesService.RemoveCollection(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(RemoveCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("track/{id}")]
        public async Task<IActionResult> RemoveTrack(int id)
        {
            try
            {
                await _itunesService.RemoveTrack(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(RemoveTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("collections")]
        public async Task<IActionResult> RemoveCollections(IEnumerable<int> ids)
        {
            try
            {
                await _itunesService.RemoveCollections(ids);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(RemoveCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("tracks")]
        public async Task<IActionResult> RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                await _itunesService.RemoveTracks(ids);
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(RemoveTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collection/{id}")]
        public async Task<IActionResult> UpdateCollection(ITunesCollection item)
        {
            try
            {
                return Ok(await _itunesService.UpdateCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collections")]
        public async Task<IActionResult> UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(await _itunesService.UpdateCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("track/{id}")]
        public async Task<IActionResult> UpdateTrack(ITunesTrack item)
        {
            try
            {
                return Ok(await _itunesService.UpdateTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("tracks")]
        public async Task<IActionResult> UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(await _itunesService.UpdateTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}