using Downgrooves.Domain.ITunes;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Test
{
    [TestClass]
    public class ITunesTests
    {
        private readonly Mock<IITunesService> _service = new();
        private readonly Mock<ILogger<ITunesController>> _mockLogger = new();

        [TestMethod]
        public void AddCollection()
        {
            var now = DateTime.Now;
            // Arrange
            var collection = GetTestCollections().FirstOrDefault();
            collection.Id = 3;
            collection.ReleaseDate = now;
            _service.Setup(x => x.AddCollection(collection)).Returns(collection);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.AddCollection(collection);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesCollection;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void AddCollections()
        {
            var now = DateTime.Now;
            // Arrange
            var collections = GetTestCollections();
            _service.Setup(x => x.AddCollections(collections)).Returns(collections);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.AddCollections(collections);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesCollection>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void AddTrack()
        {
            var now = DateTime.Now;
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.Id = 3;
            track.ReleaseDate = now;
            _service.Setup(x => x.AddTrack(track)).Returns(track);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.AddTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void AddTracks()
        {
            var now = DateTime.Now;
            // Arrange
            var tracks = GetTestTracks();
            _service.Setup(x => x.AddTracks(tracks)).Returns(tracks);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.AddTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetCollections()
        {
            // Arrange

            _service.Setup(x => x.GetCollections("Downgrooves")).Returns(GetTestCollections());

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.GetCollections("Downgrooves");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesCollection>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetCollection()
        {
            // Arrange

            _service.Setup(x => x.GetCollection(1)).Returns(GetTestCollections().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.GetCollection(1);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesCollection;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 1);
        }

        [TestMethod]
        public void GetTracks()
        {
            // Arrange

            _service.Setup(x => x.GetTracks("Downgrooves")).Returns(GetTestTracks());

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.GetTracks("Downgrooves");
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetTrack()
        {
            // Arrange

            _service.Setup(x => x.GetTrack(1)).Returns(GetTestTracks().Where(x => x.Id == 1).FirstOrDefault());

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.GetTrack(1);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 1);
        }

        [TestMethod]
        public void RemoveCollection()
        {
            // Arrange
            var collections = GetTestCollections();
            var collection = collections.FirstOrDefault();
            var id = collection.Id;
            _service.Setup(x => x.RemoveCollection(id));

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.RemoveCollection(collection.Id);
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
            _service.Setup(x => x.RemoveTrack(id));

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.RemoveTrack(track.Id);
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void RemoveCollections()
        {
            // Arrange
            var collections = GetTestCollections();
            _service.Setup(x => x.RemoveCollections(collections.Select(x => x.Id)));

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.RemoveCollections(collections.Select(x => x.Id));
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

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.RemoveTracks(tracks.Select(x => x.Id));
            var okResult = output as OkResult;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkResult));
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void UpdateCollection()
        {
            var now = DateTime.Now;
            // Arrange
            var collection = GetTestCollections().FirstOrDefault();
            collection.ReleaseDate = now;
            _service.Setup(x => x.UpdateCollection(collection)).Returns(collection);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.UpdateCollection(collection);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesCollection;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void UpdateCollections()
        {
            var now = DateTime.Now;
            // Arrange
            var collections = new List<ITunesCollection>();
            foreach (var item in GetTestCollections())
            {
                item.ReleaseDate = now;
                collections.Add(item);
            }
            _service.Setup(x => x.UpdateCollections(collections)).Returns(collections);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.UpdateCollections(collections);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesCollection>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
            foreach (var item in collections)
                Assert.AreEqual(now, item.ReleaseDate);
        }

        [TestMethod]
        public void UpdateTrack()
        {
            var now = DateTime.Now;
            // Arrange
            var track = GetTestTracks().FirstOrDefault();
            track.ReleaseDate = now;
            _service.Setup(x => x.UpdateTrack(track)).Returns(track);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.UpdateTrack(track);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as ITunesTrack;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ReleaseDate, now);
        }

        [TestMethod]
        public void UpdateTracks()
        {
            var now = DateTime.Now;
            // Arrange
            var tracks = new List<ITunesTrack>();
            foreach (var item in GetTestTracks())
            {
                item.ReleaseDate = now;
                tracks.Add(item);
            }
            _service.Setup(x => x.UpdateTracks(tracks)).Returns(tracks);

            // Act

            var iTunesController = new ITunesController(_mockLogger.Object, _service.Object);
            var output = iTunesController.UpdateTracks(tracks);
            var okResult = output as OkObjectResult;
            var result = okResult.Value as IEnumerable<ITunesTrack>;

            // Assert

            Assert.IsInstanceOfType(output, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(2, result.Count());
            foreach (var item in tracks)
                Assert.AreEqual(now, item.ReleaseDate);
        }

        private static IEnumerable<ITunesCollection> GetTestCollections()
        {
            var collections = new List<ITunesCollection>
            {
                new ITunesCollection()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 1,
                    CollectionCensoredName = "Test One"
                },
                new ITunesCollection()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 2,
                    CollectionCensoredName = "Test Two"
                }
            };
            return collections;
        }

        private static IEnumerable<ITunesTrack> GetTestTracks()
        {
            var tracks = new List<ITunesTrack>
            {
                new ITunesTrack()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 1,
                    TrackCensoredName = "Test One"
                },
                new ITunesTrack()
                {
                    ReleaseDate = new DateTime(2016, 7, 1),
                    Id = 2,
                    TrackCensoredName = "Test Two"
                }
            };
            return tracks;
        }
    }
}