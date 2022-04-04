using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    [Table("tracks")]
    public class Track
    {
        public string Artist { get; set; }
        public string Label { get; set; }

        [JsonIgnore]
        public Mix Mix { get; set; }

        public int MixId { get; set; }
        public int Number { get; set; }
        public string Remix { get; set; }
        public string Title { get; set; }
        public int TrackId { get; set; }
    }
}