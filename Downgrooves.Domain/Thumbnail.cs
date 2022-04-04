using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    [Table("thumbnails")]
    public class Thumbnail
    {
        public int Height { get; set; }
        public int ThumbnailId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public Video Video { get; set; }

        public string VideoId { get; set; }
        public int Width { get; set; }
    }
}