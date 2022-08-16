namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A row in the iTunesExclusion table.
    /// </summary>
    public class ITunesExclusion
    {
        public int? CollectionId { get; set; }
        public int? TrackId { get; set; }
    }
}