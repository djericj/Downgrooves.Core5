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
        public void Add()
        {
            var now = DateTime.Now;
            // Arrange
            var release = GetTestReleases().FirstOrDefault();
            release.Id = 3;
            release.ReleaseDate = now;
            _service.Setup(x => x.Add(release)).Returns(release);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.Add(release);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Release;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void AddTrack()
        {
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.Id = 3;
            track.Price = 1.00;
            _service.Setup(x => x.AddTrack(track)).Returns(track);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.AddTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ReleaseTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.Price, 1.00);
        }

        [TestMethod]
        public void AddTracks()
        {
            // Arrange
            var tracks = new List<ReleaseTrack>();
            foreach (var item in GetTestTracks())
            {
                item.Price = 1.00;
                tracks.Add(item);
            }
            _service.Setup(x => x.AddTracks(tracks)).Returns(tracks);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.AddTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ReleaseTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
            foreach (var item in tracks)
                Assert.AreEqual(item.Price, 1.00);
        }

        [TestMethod]
        public void GetReleases()
        {
            // Arrange

            _service.Setup(x => x.GetReleases("Downgrooves")).Returns(GetTestReleases());

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

            _service.Setup(x => x.GetReleases(x => x.Id == 1)).Returns(GetTestReleases().Where(x => x.Id == 1));

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.GetRelease(1);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Release;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Release));
        }

        [TestMethod]
        public void GetReleaseTrack()
        {
            // Arrange

            _service.Setup(x => x.GetReleaseTrack(1)).Returns(GetTestTracks().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.GetReleaseTrack(1);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ReleaseTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(ReleaseTrack));
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var releases = GetTestReleases();
            var release = releases.FirstOrDefault();
            var id = release.Id;
            _service.Setup(x => x.Remove(release.Id));

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.Remove(release.Id);
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
            var id = track.Id;
            _service.Setup(x => x.RemoveTrack(track.Id));

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.RemoveTrack(track.Id);
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
            _service.Setup(x => x.RemoveTracks(tracks.Select(x => x.Id)));

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.RemoveTracks(tracks.Select(x => x.Id));
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
            var release = GetTestReleases().FirstOrDefault();
            release.ReleaseDate = now;
            _service.Setup(x => x.Update(release)).Returns(release);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.Update(release);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Release;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void UpdateTrack()
        {
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.TrackNumber = 10;
            _service.Setup(x => x.UpdateTrack(track)).Returns(track);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.UpdateTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ReleaseTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TrackNumber, 10);
        }

        [TestMethod]
        public void UpdateTracks()
        {
            // Arrange
            var tracks = new List<ReleaseTrack>();
            foreach (var item in GetTestTracks())
            {
                item.TrackNumber = 10;
                tracks.Add(item);
            }
            _service.Setup(x => x.UpdateTracks(tracks)).Returns(tracks);

            // Act

            var releaseController = new ReleaseController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = releaseController.UpdateTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ReleaseTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            foreach (var item in tracks)
                Assert.AreEqual(item.TrackNumber, 10);
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
                    Price = 1.00,
                    Id = 1,
                    Title = "Test One"
                },
                new ReleaseTrack()
                {
                    Price = 2.00,
                    Id = 2,
                    Title = "Test Two"
                }
            };
            return tracks;
        }
    }
}