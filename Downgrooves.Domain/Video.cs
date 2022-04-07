using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
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

    [Table("video")]
    public class Video
    {
        public string Description { get; set; }

        public string ETag { get; set; }

        [Column("videoId")]
        public int Id { get; set; }

        public DateTime PublishedAt { get; set; }

        public string SourceSystemId { get; set; }

        public IList<Thumbnail> Thumbnails { get; set; }
        public string Title { get; set; }
    }
}