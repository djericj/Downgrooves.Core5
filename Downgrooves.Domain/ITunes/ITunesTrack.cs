using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain.ITunes
{
    [Table("itunestrack")]
    public class ITunesTrack : ITunesResult
    {
        [Key]
        public int TrackId { get; set; }
        public string Kind { get; set; }
        public int CollectionId { get; set; }
        public string TrackName { get; set; }
        public string TrackCensoredName { get; set; }
        public string TrackViewUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string ArtworkUrl30 { get; set; }
        public double TrackPrice { get; set; }
        public string TrackExplicitness { get; set; }
        public int DiscCount { get; set; }
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public int TrackTimeMillis { get; set; }
        public string IsStreamable { get; set; }
        [NotMapped]
        public bool IsRemix => TrackCensoredName?.Contains("Downgrooves", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}