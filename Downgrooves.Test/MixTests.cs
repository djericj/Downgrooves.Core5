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
using System.Text;
using System.Threading.Tasks;

namespace Downgrooves.Test
{
    [TestClass]
    public class MixTests
    {
        private Mock<IOptions<AppConfig>> _appConfigMock;
        private Mock<IMixService> _service = new Mock<IMixService>();
        private Mock<ILogger<MixController>> _mockLogger = new Mock<ILogger<MixController>>();

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
            mix.MixId = 3;
            mix.CreateDate = now;
            _service.Setup(x => x.Add(mix).Result).Returns(mix);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.Add(mix).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Mix;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MixId, 3);
            Assert.AreEqual(result.CreateDate, now);
        }

        [TestMethod]
        public void AddTrack()
        {
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.MixId = 3;
            track.Number = 1;
            _service.Setup(x => x.AddTrack(track).Result).Returns(track);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.AddTrack(track).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as MixTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MixId, 3);
            Assert.AreEqual(result.Number, 1);
        }

        [TestMethod]
        public void AddTracks()
        {
            // Arrange
            var tracks = GetTestTracks();
            _service.Setup(x => x.AddTracks(tracks).Result).Returns(tracks);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.AddTracks(tracks).Result;
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

            _service.Setup(x => x.GetMixes().Result).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.GetMixes().Result;
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

            _service.Setup(x => x.GetMixesByCategory("Test").Result).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.GetMixesByCategory("Test").Result;
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
            _service.Setup(x => x.GetMixesByGenre("Test").Result).Returns(GetTestMixes());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.GetMixesByGenre("Test").Result;
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

            _service.Setup(x => x.GetMix(1).Result).Returns(GetTestMixes().Where(x => x.MixId == 1).FirstOrDefault());

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.GetMix(1).Result;

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
            var id = mix.MixId;
            _service.Setup(x => x.Remove(mix.MixId));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.Remove(mix.MixId).Result;
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
            _service.Setup(x => x.RemoveTrack(track.MixTrackId));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.RemoveTrack(track.MixTrackId).Result;
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
            _service.Setup(x => x.RemoveTracks(tracks.Select(x => x.MixTrackId)));

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.RemoveTracks(tracks.Select(x => x.MixTrackId)).Result;
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
            _service.Setup(x => x.Update(mix).Result).Returns(mix);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.Update(mix).Result;
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
            _service.Setup(x => x.UpdateTrack(track).Result).Returns(track);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.UpdateTrack(track).Result;
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
            _service.Setup(x => x.UpdateTracks(tracks).Result).Returns(tracks);

            // Act

            var mixController = new MixController(_appConfigMock.Object, _mockLogger.Object, _service.Object);
            var output = mixController.UpdateTracks(tracks).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<MixTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            foreach (var item in tracks)
                Assert.AreEqual(10, item.Number);
        }

        private IEnumerable<Mix> GetTestMixes()
        {
            var mixes = new List<Mix>();
            mixes.Add(new Mix()
            {
                CreateDate = new DateTime(2016, 7, 1),
                MixId = 1,
                Title = "Test One",
                Category = "Test",
                Genre = new Genre() { Name = "Test" }
            });
            mixes.Add(new Mix()
            {
                CreateDate = new DateTime(2016, 7, 1),
                MixId = 2,
                Title = "Test Two",
                Category = "Test",
                Genre = new Genre() { Name = "Test" }
            });
            return mixes;
        }

        private IEnumerable<MixTrack> GetTestTracks()
        {
            var tracks = new List<MixTrack>();
            tracks.Add(new MixTrack()
            {
                Number = 1,
                MixTrackId = 1,
                Title = "Test One"
            });
            tracks.Add(new MixTrack()
            {
                Number = 2,
                MixTrackId = 2,
                Title = "Test Two"
            });
            return tracks;
        }
    }
}