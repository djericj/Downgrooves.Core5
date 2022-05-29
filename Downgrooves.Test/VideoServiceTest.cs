using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service;
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
using System.Threading.Tasks;

namespace Downgrooves.Test
{
    [TestClass]
    public class VideoServiceTest
    {
        private Mock<IOptions<AppConfig>> _appConfigMock;
        private Mock<IVideoService> _service = new Mock<IVideoService>();
        private Mock<ILogger<VideoController>> _mockLogger = new Mock<ILogger<VideoController>>();

        [TestInitialize]
        public void Init()
        {
            var appConfig = new AppConfig() { CdnUrl = "" };
            _appConfigMock = new Mock<IOptions<AppConfig>>();
            _appConfigMock.Setup(x => x.Value).Returns(appConfig);
        }

        [TestMethod]
        public void AddVideo()
        {
            var now = DateTime.Now;
            // Arrange
            var video = GetTestVideos().FirstOrDefault();
            video.Id = 3;
            video.PublishedAt = now;
            _service.Setup(x => x.Add(video).Result).Returns(video);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.Add(video);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Task<Video>;
            var v = result.Result;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Id, 3);
            Assert.AreEqual(v.PublishedAt, now);
        }

        [TestMethod]
        public void AddRange()
        {
            var mockSet = new Mock<IVideoRepository>();
            var mockContext = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<VideoService>>();
            mockContext.Setup(m => m.Videos).Returns(mockSet.Object);

            var service = new VideoService(mockContext.Object, mockLogger.Object);
            service.AddRange(GetTestVideos());

            mockSet.Verify(m => m.AddRange(It.IsAny<IEnumerable<Video>>()), Times.Once());
            mockContext.Verify(m => m.CompleteAsync(), Times.Once());
        }

        [TestMethod]
        public void GetVideo()
        {
            // Arrange

            _service.Setup(x => x.GetVideo("1").Result).Returns(GetTestVideos().Where(x => x.SourceSystemId == "1").FirstOrDefault());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetVideo("1").Result;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Video));
        }

        [TestMethod]
        public void GetVideos()
        {
            // Arrange

            _service.Setup(x => x.GetVideos().Result).Returns(GetTestVideos());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetVideos().Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Video>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var videos = GetTestVideos();
            var video = videos.FirstOrDefault();
            var id = video.Id;
            _service.Setup(x => x.Remove(video.SourceSystemId));

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.Remove(video.SourceSystemId).Result;
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
            var video = GetTestVideos().FirstOrDefault();
            video.PublishedAt = now;
            _service.Setup(x => x.Update(video).Result).Returns(video);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.Update(video);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Task<Video>;
            var v = result.Result;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.PublishedAt, now);
        }

        private IEnumerable<Video> GetTestVideos()
        {
            var videos = new List<Video>();
            videos.Add(new Video()
            {
                PublishedAt = new DateTime(2016, 7, 2),
                Id = 1,
                SourceSystemId = "1",
                Title = "Test One"
            });
            videos.Add(new Video()
            {
                PublishedAt = new DateTime(2016, 7, 1),
                Id = 2,
                SourceSystemId = "2",
                Title = "Test Two"
            });
            return videos;
        }
    }
}