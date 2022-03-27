using System;
using System.Collections.Generic;

namespace Downgrooves.Domain
{
    public class Release
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string CollectionName { get; set; }
        public string TrackName { get; set; }
        public string CollectionCensoredName { get; set; }
        public string TrackCensoredName { get; set; }
        public string ArtistViewUrl { get; set; }
        public string CollectionViewUrl { get; set; }
        public string TrackViewUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string ArtworkUrl30 { get; set; }
        public string ArtworkUrl60 { get; set; }
        public string ArtworkUrl100 { get; set; }
        public double CollectionPrice { get; set; }
        public double TrackPrice { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TrackCount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string PrimaryGenreName { get; set; }
        public bool IsOriginal => ArtistName.Contains("Downgrooves", StringComparison.OrdinalIgnoreCase);
        public ICollection<ITunesTrack> Tracks { get; set; }
    }
}
