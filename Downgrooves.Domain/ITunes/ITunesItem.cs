using System;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A base class for common properties of collections and tracks from the iTunes API.
    /// </summary>
    public abstract class ITunesItem
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string CollectionCensoredName { get; set; }
        public string ArtistViewUrl { get; set; }
        public string CollectionViewUrl { get; set; }
        public string ArtworkUrl60 { get; set; }
        public string ArtworkUrl100 { get; set; }
        public double? CollectionPrice { get; set; }
        public string CollectionExplicitness { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public virtual int Id { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PrimaryGenreName { get; set; }
        public int TrackCount { get; set; }
        public string WrapperType { get; set; }
    }
}