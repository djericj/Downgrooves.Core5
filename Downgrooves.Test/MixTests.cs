using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
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
    public class MixTests
    {
        private Mock<IOptions<AppConfig>> _appConfigMock;
        private readonly Mock<IMixService> _mixService = new();
        private readonly Mock<ILogger<MixController>> _mockLogger = new();

        [TestInitialize]
        public void Init()
        {
            var appConfig = new AppConfig() { CdnUrl = "" };
            _appConfigMock = new Mock<IOptions<AppConfig>>();
            _appConfigMock.Setup(x => x.Value).Returns(appConfig);
        }
        
        [TestMethod]
        public void GetMixes()
        {
            // Arrange

            _mixService.Setup(x => x.GetAll()).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixes();
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType<OkObjectResult>(output);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMixesByCategory()
        {
            // Arrange

            _mixService.Setup(x => x.GetByCategory("Test")).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixesByCategory("Test");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType<OkObjectResult>(output);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMixesByGenre()
        {
            // Arrange
            var genre = new Genre() { Name = "Test" };
            _mixService.Setup(x => x.GetByGenre("Test")).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixesByGenre("Test");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType<OkObjectResult>(output);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMix()
        {
            // Arrange

            _mixService.Setup(x => x.GetMix(1)).Returns(GetTestMixes().FirstOrDefault(x => x.Id == 1));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMix(1);

            // Assert

            Assert.IsInstanceOfType<OkObjectResult>(output);
            Assert.IsInstanceOfType<Mix>(((OkObjectResult)output).Value);
        }

        private static List<Mix> GetTestMixes()
        {
            var mixes = new List<Mix>
            {
                new()
                {
                    CreateDate = new DateTime(2016, 7, 1),
                    Id = 1,
                    Title = "Test One",
                    Category = "Test",
                    Genre = new Genre() { Name = "Test" }
                },
                new()
                {
                    CreateDate = new DateTime(2016, 7, 1),
                    Id = 2,
                    Title = "Test Two",
                    Category = "Test",
                    Genre = new Genre() { Name = "Test" }
                }
            };
            return mixes;
        }

        private static List<MixTrack> GetTestTracks()
        {
            var tracks = new List<MixTrack>
            {
                new()
                {
                    Number = 1,
                    Id = 1,
                    Title = "Test One"
                },
                new()
                {
                    Number = 2,
                    Id = 2,
                    Title = "Test Two"
                }
            };
            return tracks;
        }
    }
}