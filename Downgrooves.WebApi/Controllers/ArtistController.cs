using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("artists")]
    public class ArtistController : ControllerBase
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ArtistController> _logger;
        private readonly IArtistService _service;

        public ArtistController(IOptions<AppConfig> config, ILogger<ArtistController> logger, IArtistService service)
        {
            _appConfig = config.Value;
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetArtists()
        {
            try
            {
                return Ok(_service.GetArtists());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ArtistController)}.{nameof(GetArtists)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Route("releases")]
        public IActionResult GetArtistsAndReleases()
        {
            try
            {
                var artists = _service.GetArtistsAndReleases() as List<Artist>;
                artists.ForEach(x => x.Releases.SetBasePath(_appConfig.CdnUrl));
                return Ok(artists);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in {nameof(ArtistController)}.{nameof(GetArtistsAndReleases)} {ex.Message} {ex.StackTrace}");
                return StatusCode(500, $"{ex.Message} StackTrace: {ex.StackTrace}");
            }
        }
    }
}