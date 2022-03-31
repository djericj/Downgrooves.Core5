using Newtonsoft.Json;
using System.Collections.Generic;

namespace Downgrooves.Domain.ITunes
{
    public class ITunesLookupTrackResult : ITunesLookupResult
    {
        [JsonProperty("results")]
        public IEnumerable<ITunesTrack> Results { get; set; }
    }
}
