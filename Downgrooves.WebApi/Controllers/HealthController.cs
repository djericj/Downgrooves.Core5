using System;
using System.Collections.Generic;
using Downgrooves.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Downgrooves.WebApi.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IHealthService _service;

        public HealthController(ILogger<HealthController> logger, IHealthService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            try
            {
                var report = _service.GetHealth();

                var empty = new List<string>();
                if (report.Artists == 0) empty.Add("artists");
                if (report.Genres == 0) empty.Add("genres");
                if (report.Mixes == 0) empty.Add("mixes");
                if (report.Releases == 0) empty.Add("releases");

                var body = new
                {
                    status = empty.Count == 0 ? "healthy" : "unhealthy",
                    reason = empty.Count == 0 ? null : $"No data for: {string.Join(", ", empty)}",
                    artists = report.Artists,
                    genres = report.Genres,
                    mixes = report.Mixes,
                    releases = report.Releases,
                };

                if (empty.Count > 0)
                {
                    _logger.LogWarning("Health check unhealthy: {Reason}", body.reason);
                    return StatusCode(503, body);
                }

                return Ok(body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new { status = "unhealthy", reason = "Failed to load data" });
            }
        }
    }
}
