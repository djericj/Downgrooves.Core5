using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ReleaseService> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;
        private readonly IITunesService _itunesService;

        public ReleaseService(IOptions<AppConfig> config,
            ILogger<ReleaseService> logger,
            IApiDataService apiDataService,
            IArtistService artistService,
            IArtworkService artworkService,
            IITunesService itunesService)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiDataService = apiDataService;
            _artistService = artistService;
            _artworkService = artworkService;
            _itunesService = itunesService;
        }
    }
}