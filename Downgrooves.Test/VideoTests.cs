using Downgrooves.Model;
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
    public class VideoTests
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
            _service.Setup(x => x.AddVideo(video).Result).Returns(video);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.Add(video).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Video;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.PublishedAt, now);
        }

        [TestMethod]
        public void AddVideos()
        {
            var now = DateTime.Now;
            // Arrange
            var videos = GetTestVideos();
            _service.Setup(x => x.AddVideos(videos).Result).Returns(videos);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.AddVideos(videos).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Video>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetVideo()
        {
            // Arrange

            _service.Setup(x => x.GetVideo(1).Result).Returns(GetTestVideos().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetVideo(1).Result;

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
            _service.Setup(x => x.Remove(video.Id));

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.Remove(video.Id).Result;
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
            _service.Setup(x => x.UpdateVideo(video).Result).Returns(video);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.UpdateVideo(video).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Video;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.PublishedAt, now);
        }

        [TestMethod]
        public void AddThumbnail()
        {
            // Arrange
            var thumbnail = GetTestThumbnails().FirstOrDefault();
            thumbnail.Id = 3;
            _service.Setup(x => x.AddThumbnail(3, thumbnail).Result).Returns(thumbnail);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.AddThumbnail(3, thumbnail).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Thumbnail;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
        }

        [TestMethod]
        public void AddThumbnails()
        {
            // Arrange
            var thumbnails = GetTestThumbnails();
            _service.Setup(x => x.AddThumbnails(3, thumbnails).Result).Returns(thumbnails);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.AddThumbnails(3, thumbnails).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Thumbnail>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetThumbnails()
        {
            // Arrange
            var video = new Video() { Id = 1 };
            _service.Setup(x => x.GetThumbnails(3).Result).Returns(GetTestThumbnails());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetThumbnails(3).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Thumbnail>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetThumbnail()
        {
            // Arrange
            var video = new Video() { Id = 1 };
            _service.Setup(x => x.GetThumbnail(1).Result).Returns(GetTestThumbnails().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetThumbnail(1).Result;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Thumbnail));
        }

        [TestMethod]
        public void RemoveThumbnail()
        {
            // Arrange
            var thumbnails = GetTestThumbnails();
            var thumbnail = thumbnails.FirstOrDefault();
            var id = thumbnail.Id;
            _service.Setup(x => x.RemoveThumbnail(thumbnail.Id));

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.RemoveThumbnail(thumbnail.Id).Result;
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void RemoveThumbnails()
        {
            // Arrange
            var thumbnails = GetTestThumbnails();
            _service.Setup(x => x.RemoveThumbnails(thumbnails.Select(x => x.Id)));

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.RemoveThumbnails(3, thumbnails.Select(x => x.Id)).Result;
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void UpdateThumbnail()
        {
            // Arrange
            var thumbnail = GetTestThumbnails().FirstOrDefault();
            _service.Setup(x => x.UpdateThumbnail(3, thumbnail).Result).Returns(thumbnail);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.UpdateThumbnail(3, thumbnail).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as Thumbnail;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateThumbnails()
        {
            // Arrange
            var thumbnails = GetTestThumbnails();
            _service.Setup(x => x.UpdateThumbnails(3, thumbnails).Result).Returns(thumbnails);

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.UpdateThumbnails(3, thumbnails).Result;
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Thumbnail>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
        }

        private IEnumerable<Video> GetTestVideos()
        {
            var videos = new List<Video>();
            videos.Add(new Video()
            {
                PublishedAt = new DateTime(2016, 7, 2),
                Id = 1,
                Title = "Test One"
            });
            videos.Add(new Video()
            {
                PublishedAt = new DateTime(2016, 7, 1),
                Id = 2,
                Title = "Test Two"
            });
            return videos;
        }

        private IEnumerable<Thumbnail> GetTestThumbnails()
        {
            var thumbnail = new List<Thumbnail>();
            thumbnail.Add(new Thumbnail()
            {
                VideoId = 3,
                Id = 1,
                Url = "Test One"
            });
            thumbnail.Add(new Thumbnail()
            {
                VideoId = 3,
                Id = 2,
                Url = "Test Two"
            });
            return thumbnail;
        }
    }
}