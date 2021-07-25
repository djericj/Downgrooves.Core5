using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    public class Thumbnail
    {
        public int ThumbnailId { get; set; }
        public string VideoId { get; set; }

        [JsonIgnore]
        public Video Video { get; set; }

        public string Type { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}