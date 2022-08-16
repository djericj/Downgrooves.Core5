namespace Downgrooves.Domain.YouTube
{
    /// <summary>
    /// An item of the items node in the YouTube API response.
    /// </summary>
    public class YouTubeVideo
    {
        public string ETag { get; set; }

        public string Id { get; set; }

        public Snippet Snippet { get; set; }
    }
}