using Downgrooves.Domain.ITunes;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
        public IActionResult AddCollection(ITunesCollection item)
        {
            try
            {
                return Ok(_itunesService.AddCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("collections")]
        public IActionResult AddCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(_itunesService.AddCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("track")]
        public IActionResult AddTrack(ITunesTrack item)
        {
            try
            {
                return Ok(_itunesService.AddTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("tracks")]
        public IActionResult AddTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(_itunesService.AddTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("collections")]
        public IActionResult GetCollections([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(_itunesService.GetCollections(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("collection/{id}")]
        public IActionResult GetCollection(int id)
        {
            try
            {
                return Ok(_itunesService.GetCollection(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("exclusions")]
        public IActionResult GetExclusions()
        {
            try
            {
                return Ok(_itunesService.GetExclusions());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetExclusions)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("lookup")]
        public IActionResult GetLookup()
        {
            try
            {
                return Ok(_itunesService.GetLookupLog());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetLookup)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("lookup/{id}")]
        public IActionResult GetLookup(int id)
        {
            try
            {
                return Ok(_itunesService.GetLookupLog(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetLookup)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Route("lookup")]
        public IActionResult AddLookup(ITunesLookupLog item)
        {
            try
            {
                return Ok(_itunesService.AddLookupLog(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(AddLookup)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("tracks")]
        public IActionResult GetTracks([FromQuery] string artistName = null)
        {
            try
            {
                return Ok(_itunesService.GetTracks(artistName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("track/{id}")]
        public IActionResult GetTrack(int id)
        {
            try
            {
                return Ok(_itunesService.GetTrack(id));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(GetTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [Route("collection/{id}")]
        public IActionResult RemoveCollection(int id)
        {
            try
            {
                _itunesService.RemoveCollection(id);
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
        public IActionResult RemoveTrack(int id)
        {
            try
            {
                _itunesService.RemoveTrack(id);
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
        public IActionResult RemoveCollections(IEnumerable<int> ids)
        {
            try
            {
                _itunesService.RemoveCollections(ids);
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
        public IActionResult RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                _itunesService.RemoveTracks(ids);
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
        public IActionResult UpdateCollection(ITunesCollection item)
        {
            try
            {
                return Ok(_itunesService.UpdateCollection(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateCollection)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collections")]
        public IActionResult UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                return Ok(_itunesService.UpdateCollections(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateCollections)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("track/{id}")]
        public IActionResult UpdateTrack(ITunesTrack item)
        {
            try
            {
                return Ok(_itunesService.UpdateTrack(item));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateTrack)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("tracks")]
        public IActionResult UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                return Ok(_itunesService.UpdateTracks(items));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ITunesController)}.{nameof(UpdateTracks)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}