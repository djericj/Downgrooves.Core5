using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Downgrooves.Domain
{
    /// <summary>
    /// An online video.
    /// </summary>
    public class Video
    {
        public string ArtworkUrl { get; set; }
        public string BasePath { get; set; }

        public string Description { get; set; }
        [JsonProperty("videoId")]
        public int Id { get; set; }
        public DateTime PublishedAt { get; set; }

        public string SourceSystemId { get; set; }
        public IList<Thumbnail> Thumbnails { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
    }
}