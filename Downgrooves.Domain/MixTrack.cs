using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the mixTrack table
    /// </summary>
    [Table("mixTrack")]
    public class MixTrack
    {
        public string Artist { get; set; }
        public string Label { get; set; }

        [JsonIgnore]
        public Mix Mix { get; set; }

        public int MixId { get; set; }

        [Key]
        [Column("trackId")]
        public int MixTrackId { get; set; }

        public int Number { get; set; }
        public string Remix { get; set; }
        public string Title { get; set; }
    }
}