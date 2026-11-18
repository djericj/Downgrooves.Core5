namespace Downgrooves.Domain
{
    /// <summary>
    /// A track on a release.
    /// </summary>
    public class ReleaseTrack
    {
        public string ArtistName { get; set; }
        public long Id { get; set; }
        public string PreviewUrl { get; set; }
        public decimal Price { get; set; }
        public long ReleaseId { get; set; }
        public string Title { get; set; }
        public long TrackId { get; set; }
        public int TrackNumber { get; set; }
        public int TrackTimeInMilliseconds { get; set; }
    }
}