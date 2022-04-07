using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Downgrooves.Domain.YouTube
{
    /// <summary>
    /// An item of the items node in the YouTube API response.
    /// </summary>
    public class YouTubeVideo
    {
        public string ETag { get; set; }

        public string Id { get; set; }

        public Snippet Snippet { get; set; }
    }

    public class Snippet
    {
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public Resource ResourceId { get; set; }

        [JsonProperty("thumbnails")]
        public Thumbnails Thumbnails { get; set; }

        public string Title { get; set; }
    }

    public class Thumbnails
    {
        public ThumbnailImage Default { get; set; }

        public ThumbnailImage High { get; set; }

        [JsonProperty("maxres")]
        public ThumbnailImage MaxResolution { get; set; }

        public ThumbnailImage Medium { get; set; }

        public ThumbnailImage Standard { get; set; }
    }

    public class ThumbnailImage
    {
        public int Height { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }

    public class Resource
    {
        public string Kind { get; set; }
        public string VideoId { get; set; }
    }
}