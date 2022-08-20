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
        public void Add()
        {
            var now = DateTime.Now;
            // Arrange
            var mix = GetTestMixes().FirstOrDefault();
            mix.Id = 3;
            mix.CreateDate = now;
            _mixService.Setup(x => x.Add(mix)).Returns(mix);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.Add(mix);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Mix;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.CreateDate, now);
        }

        [TestMethod]
        public void AddTrack()
        {
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.Id = 3;
            track.Number = 1;
            _mixService.Setup(x => x.AddTrack(track)).Returns(track);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.AddTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as MixTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.Number, 1);
        }

        [TestMethod]
        public void AddTracks()
        {
            // Arrange
            var tracks = GetTestTracks();
            _mixService.Setup(x => x.AddTracks(tracks)).Returns(tracks);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.AddTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<MixTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMixes()
        {
            // Arrange

            _mixService.Setup(x => x.GetMixes()).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixes();
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMixesByCategory()
        {
            // Arrange

            _mixService.Setup(x => x.GetMixesByCategory("Test")).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixesByCategory("Test");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMixesByGenre()
        {
            // Arrange
            var genre = new Genre() { Name = "Test" };
            _mixService.Setup(x => x.GetMixesByGenre("Test")).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMixesByGenre("Test");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Mix>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMix()
        {
            // Arrange

            _mixService.Setup(x => x.GetMix(1)).Returns(GetTestMixes().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.GetMix(1);

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Mix));
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var mixes = GetTestMixes();
            var mix = mixes.FirstOrDefault();
            var id = mix.Id;
            _mixService.Setup(x => x.Remove(mix.Id));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.Remove(mix.Id);
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void RemoveTrack()
        {
            // Arrange
            var tracks = GetTestTracks();
            var track = tracks.FirstOrDefault();
            _mixService.Setup(x => x.RemoveTrack(track.Id));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.RemoveTrack(track.Id);
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void RemoveTracks()
        {
            // Arrange
            var tracks = GetTestTracks();
            _mixService.Setup(x => x.RemoveTracks(tracks.Select(x => x.Id)));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.RemoveTracks(tracks.Select(x => x.Id));
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void Update()
        {
            var now = DateTime.Now;
            // Arrange
            var mix = GetTestMixes().FirstOrDefault();
            mix.CreateDate = now;
            _mixService.Setup(x => x.Update(mix)).Returns(mix);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.Update(mix);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Mix;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.CreateDate, now);
        }

        [TestMethod]
        public void UpdateTrack()
        {
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.Number = 10;
            _mixService.Setup(x => x.UpdateTrack(track)).Returns(track);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.UpdateTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as MixTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Number, 10);
        }

        [TestMethod]
        public void UpdateTracks()
        {
            // Arrange
            var tracks = new List<MixTrack>();
            foreach (var item in GetTestTracks())
            {
                item.Number = 10;
                tracks.Add(item);
            }
            _mixService.Setup(x => x.UpdateTracks(tracks)).Returns(tracks);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _mixService.Object);
            var output = mixController.UpdateTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<MixTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            foreach (var item in tracks)
                Assert.AreEqual(10, item.Number);
        }

        private static IEnumerable<Mix> GetTestMixes()
        {
            var mixes = new List<Mix>
            {
                new Mix()
                {
                    CreateDate = new DateTime(2016, 7, 1),
                    Id = 1,
                    Title = "Test One",
                    Category = "Test",
                    Genre = new Genre() { Name = "Test" }
                },
                new Mix()
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

        private static IEnumerable<MixTrack> GetTestTracks()
        {
            var tracks = new List<MixTrack>
            {
                new MixTrack()
                {
                    Number = 1,
                    Id = 1,
                    Title = "Test One"
                },
                new MixTrack()
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