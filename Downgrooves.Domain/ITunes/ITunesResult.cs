using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain.ITunes
{
    public class ITunesResult
    {
        public string WrapperType { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string CollectionName { get; set; }
        public string CollectionCensoredName { get; set; }
        public string ArtistViewUrl { get; set; }
        public string CollectionViewUrl { get; set; }
        public string ArtworkUrl60 { get; set; }
        public string ArtworkUrl100 { get; set; }
        public double CollectionPrice { get; set; }
        public string CollectionExplicitness { get; set; }
        public int TrackCount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PrimaryGenreName { get; set; }
        [NotMapped]
        public bool IsOriginal => ArtistName?.Contains("Downgrooves", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
