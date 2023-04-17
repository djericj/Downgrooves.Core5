using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Downgrooves.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Test
{
    [TestClass]
    public class ReleaseTests
    {
        private Mock<IOptions<AppConfig>> _appConfigMock;
        private readonly Mock<IReleaseService> _service = new();
        private readonly Mock<ILogger<ReleaseController>> _mockLogger = new();

        [TestInitialize]
        public void Init()
        {
            var appConfig = new AppConfig() { CdnUrl = "" };
            _appConfigMock = new Mock<IOptions<AppConfig>>();
            _appConfigMock.Setup(x => x.Value).Returns(appConfig);
        }

        
        [TestMethod]
        public void GetReleases()
        {
            // Arrange

            _service.Setup(x => x.GetAll(x => x.ArtistName == "Downgrooves")).Returns(GetTestReleases());

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.GetReleases("Downgrooves");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Release>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetRelease()
        {
            // Arrange

            _service.Setup(x => x.GetAll(x => x.Id == 1)).Returns(GetTestReleases().Where(x => x.Id == 1));

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.GetRelease(1);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Release;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Release));
        }
        
        private static IEnumerable<Release> GetTestReleases()
        {
            var releases = new List<Release>
            {
                new Release()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 1,
                    Title = "Test One"
                },
                new Release()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 2,
                    Title = "Test Two"
                }
            };
            return releases;
        }

        private static IEnumerable<ReleaseTrack> GetTestTracks()
        {
            var tracks = new List<ReleaseTrack>
            {
                new ReleaseTrack()
                {
                    Price = new decimal(1.00),
                    Id = 1,
                    Title = "Test One"
                },
                new ReleaseTrack()
                {
                    Price = new decimal(2.00),
                    Id = 2,
                    Title = "Test Two"
                }
            };
            return tracks;
        }
    }
}