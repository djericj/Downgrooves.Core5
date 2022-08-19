using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Downgrooves.WorkerService.Services;
using Microsoft.Extensions.Configuration;

namespace Downgrooves.Test
{
    [TestClass]
    public class JsonTests
    {
        private ILogger<ReleaseService> _logger;
        private IOptions<AppConfig> _options;
        private readonly IApiDataService _apiDataService;
        private IArtistService _artistService;
        private IArtworkService _artworkService;
        private IITunesService _itunesService;

        [TestInitialize]
        public void Init()
        {
            _logger = Substitute.For<ILogger<ReleaseService>>();

            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<AppConfig>()
            .Build();

            _options = Substitute.For<IOptions<AppConfig>>();
            _options.Value.Returns(new AppConfig() { ApiUrl = "http://localhost:5000/", Token = configuration.GetValue<string>("AppConfig:Token") });

            //_apiService = new ApiService(_options, _logger);
            _artistService = new ArtistService(_options, Substitute.For<ILogger<ArtistService>>());
            _artworkService = new ArtworkService(_options, Substitute.For<ILogger<ArtworkService>>());
            _itunesService = new ITunesService(_options, Substitute.For<ILogger<ITunesService>>());
        }

        public void Test1()
        {
            var releaseService = new ReleaseService(_options, _logger, _apiDataService, _artistService, _artworkService, _itunesService);
            releaseService.ProcessData();
        }
    }
}