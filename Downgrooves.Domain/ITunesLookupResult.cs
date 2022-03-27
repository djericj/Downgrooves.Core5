using Newtonsoft.Json;
using System.Collections.Generic;

namespace Downgrooves.Domain
{
    public class ITunesLookupResult
    {
        [JsonProperty("resultCount")]
        public int ResultCount { get; set; }
        [JsonProperty("results")]
        public IEnumerable<ITunesTrack> Results { get; set; }
    }
}
