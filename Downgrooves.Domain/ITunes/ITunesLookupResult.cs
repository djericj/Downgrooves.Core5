using System.Collections.Generic;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// The response of an iTunes API request.
    /// </summary>
    public class ITunesLookupResult
    {
        public int ResultCount { get; set; }
        public IEnumerable<ITunesLookupResultItem> Results { get; set; }
    }
}