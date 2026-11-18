namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A row in the iTunesExclusion table.
    /// </summary>
    public class ITunesExclusion
    {
        public long? CollectionId { get; set; }
        public long? TrackId { get; set; }
    }
}