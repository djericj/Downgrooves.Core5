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
    public class VideoTests
    {
        private Mock<IOptions<AppConfig>> _appConfigMock;
        private readonly Mock<IVideoService> _service = new();
        private readonly Mock<ILogger<VideoController>> _mockLogger = new();

        [TestInitialize]
        public void Init()
        {
            var appConfig = new AppConfig() { CdnUrl = "" };
            _appConfigMock = new Mock<IOptions<AppConfig>>();
            _appConfigMock.Setup(x => x.Value).Returns(appConfig);
        }

        [TestMethod]
        public void GetVideo()
        {
            // Arrange

            _service.Setup(x => x.GetVideo(1)).Returns(GetTestVideos().FirstOrDefault(x => x.Id == 1));

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetVideo(1);

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)output).Value, typeof(Video));
        }

        [TestMethod]
        public void GetVideos()
        {
            // Arrange

            _service.Setup(x => x.GetVideos()).Returns(GetTestVideos());

            // Act

            var videoController = new VideoController(_appConfigMock.Object, _service.Object, _mockLogger.Object);
            var output = videoController.GetVideos();
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<Video>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }
        
        private static IEnumerable<Video> GetTestVideos()
        {
            var videos = new List<Video>
            {
                new Video()
                {
                    PublishedAt = new DateTime(2016, 7, 2),
                    Id = 1,
                    Title = "Test One"
                },
                new Video()
                {
                    PublishedAt = new DateTime(2016, 7, 1),
                    Id = 2,
                    Title = "Test Two"
                }
            };
            return videos;
        }

        private static IEnumerable<Thumbnail> GetTestThumbnails()
        {
            var thumbnail = new List<Thumbnail>
            {
                new Thumbnail()
                {
                    VideoId = 3,
                    Id = 1,
                    Url = "Test One"
                },
                new Thumbnail()
                {
                    VideoId = 3,
                    Id = 2,
                    Url = "Test Two"
                }
            };
            return thumbnail;
        }
    }
}