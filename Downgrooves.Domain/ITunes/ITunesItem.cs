using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A base class for common properties of collections and tracks from the iTunes API.
    /// </summary>
    public abstract class ITunesItem
    {
        private string _collectionCensoredName;

        public int ArtistId { get; set; }
        public string ArtistName { get; set; }

        [NotMapped]
        public string CollectionName { get => _collectionCensoredName; }

        public string CollectionCensoredName { get => _collectionCensoredName; set => _collectionCensoredName = value; }
        public string ArtistViewUrl { get; set; }
        public string CollectionViewUrl { get; set; }
        public string ArtworkUrl60 { get; set; }
        public string ArtworkUrl100 { get; set; }
        public double? CollectionPrice { get; set; }
        public string CollectionExplicitness { get; set; }
        public int TrackCount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PrimaryGenreName { get; set; }
        public int? SourceArtistId { get; set; }
    }
}