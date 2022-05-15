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
        public async Task<IActionResult> GetArtists() => Ok(await _service.GetArtists());

        [HttpGet]
        [Route("releases")]
        public async Task<IActionResult> GetArtistsAndReleases()
        {
            var artists = await _service.GetArtistsAndReleases() as List<Artist>;
            artists.ForEach(x => x.Releases.SetBasePath(_appConfig.CdnUrl));
            return Ok(artists);
        }
    }
}