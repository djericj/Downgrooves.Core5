using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the thumbnail table.
    /// </summary>
    [Table("thumbnail")]
    public class Thumbnail
    {
        public int ThumbnailId { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        [JsonIgnore]
        public Video Video { get; set; }

        public int VideoId { get; set; }
    }

    /// <summary>
    /// A row in the video table.
    /// </summary>
    [Table("video")]
    public class Video
    {
        private string _basePath;

        [NotMapped]
        [JsonIgnore]
        public string BasePath { get => _basePath; set => _basePath = value; }

        public DateTime PublishedAt { get; set; }

        public IList<Thumbnail> Thumbnails { get; set; }

        [Column("videoId")]
        public int Id { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string ArtworkPath
        {
            get
            {
                return $"{_basePath}/images/artwork/videos/{SourceSystemId}";
            }
        }

        public string Default => $"{ArtworkPath}/default.jpg";
        public string Description { get; set; }
        public string ETag { get; set; }
        public string High => $"{ArtworkPath}/high.jpg";
        public string MaxRes => $"{ArtworkPath}/maxres.jpg";
        public string Medium => $"{ArtworkPath}/medium.jpg";
        public string SourceSystemId { get; set; }
        public string Standard => $"{ArtworkPath}/standard.jpg";
        public string Title { get; set; }
    }
}