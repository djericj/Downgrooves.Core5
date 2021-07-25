using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("videos")]
    public class Video
    {
        public string VideoId { get; set; }
        public string ETag { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Thumbnail> Thumbnails { get; set; }
    }
}