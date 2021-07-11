using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("tracks")]
    public class Track
    {
        public int TrackId { get; set; }
        public int MixId { get; set; }
        public int Number { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Remix { get; set; }
        public string Label { get; set; }
    }
}