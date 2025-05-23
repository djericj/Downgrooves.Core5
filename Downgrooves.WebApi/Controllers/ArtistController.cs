﻿using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downgrooves.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("artists")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IArtistService _service;

        public ArtistController(IOptions<AppConfig> config, ILogger<ArtistController> logger, IArtistService service)
        {
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
    }
}