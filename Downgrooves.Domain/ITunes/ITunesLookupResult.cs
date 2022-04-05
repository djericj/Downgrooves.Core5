using Newtonsoft.Json;
using System.Collections.Generic;

namespace Downgrooves.Domain.ITunes
{
    public class ITunesLookupResult
    {
        [JsonProperty("resultCount")]
        public int ResultCount { get; set; }

        [JsonProperty("results")]
        public IEnumerable<ITunesLookupResultItem> Results { get; set; }
    }
}