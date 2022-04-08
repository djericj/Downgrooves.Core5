using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("artists")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IArtistService _service;

        public ArtistController(ILogger<ArtistController> logger, IArtistService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists() => Ok(await _service.GetArtists());
    }
}