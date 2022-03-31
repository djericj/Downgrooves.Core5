using Newtonsoft.Json;

namespace Downgrooves.Domain.ITunes
{
    public abstract class ITunesLookupResult
    {
        [JsonProperty("resultCount")]
        public int ResultCount { get; set; }
    }
}
