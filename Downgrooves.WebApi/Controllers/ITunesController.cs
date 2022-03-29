using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("itunes")]
    public class ITunesController : ControllerBase
    {
        private readonly IITunesService _service;

        public ITunesController(IITunesService service)
        {
            _service = service;
        }

        #region Collections


        [HttpGet]
        [Route("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] string artistName = null)
        {
            return Ok(await _service.GetCollections(artistName));
        }

        [HttpGet]
        [Route("collections/paged")]
        public async Task<IActionResult> GetCollectionsAsync([FromQuery] PagingParameters parameters, string artistName = null)
        {
            return Ok(await _service.GetCollections(parameters, artistName));
        }

        [HttpGet]
        [Route("collections/lookup/{collectionId}")]
        public async Task<IActionResult> LookupCollection(int collectionId)
        {
            return Ok(await _service.LookupCollection(collectionId));
        }

        [HttpPost]
        [Route("collections")]
        public async Task<IActionResult> AddCollection(ITunesCollection collection)
        {
            try
            {
                return Ok(await _service.Add(collection));
            }
            catch (System.Exception ex)
            {
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("collections")]
        public async Task<IActionResult> UpdateCollection(ITunesCollection collection)
        {
            try
            {
                return Ok(await _service.Update(collection));
            }
            catch (System.Exception ex)
            {
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }            
        }

        #endregion

        #region Tracks

        [HttpGet]
        [Route("tracks")]        
        public async Task<IActionResult> GetTracks([FromQuery] string artistName = null)
        {
            return Ok(await _service.GetTracks(artistName));
        }

        [HttpGet]
        [Route("tracks/paged")]
        public async Task<IActionResult> GetTracksAsync([FromQuery] PagingParameters parameters, string artistName = null)
        {
            return Ok(await _service.GetTracks(parameters, artistName));
        }

        [HttpGet]
        [Route("tracks/lookup/{collectionId}")]
        public async Task<IActionResult> LookupTracks(int collectionId)
        {
            return Ok(await _service.LookupTracks(collectionId));
        }

        [HttpPost]
        [Route("tracks")]
        public async Task<IActionResult> AddTrack(ITunesTrack track)
        {
            try
            {
                return Ok(await _service.Add(track));
            }
            catch (System.Exception ex)
            {
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }            
        }

        [HttpPut]
        [Route("tracks")]
        public async Task<IActionResult> UpdateTrack(ITunesTrack track)
        {
            try
            {
                return Ok(await _service.Update(track));
            }
            catch (System.Exception ex)
            {
                return BadRequest($"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        #endregion
    }
}