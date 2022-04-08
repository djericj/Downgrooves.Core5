using System.Collections.Generic;

namespace Downgrooves.Domain.YouTube
{
    /// <summary>
    /// The response of a YouTube API request.
    /// </summary>
    public class YouTubeLookupResult
    {
        public string Kind { get; set; }
        public string ETag { get; set; }
        public IList<YouTubeVideo> Items { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class PageInfo
    {
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }
    }
}