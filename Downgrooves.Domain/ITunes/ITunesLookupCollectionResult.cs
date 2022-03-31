using Newtonsoft.Json;
using System.Collections.Generic;

namespace Downgrooves.Domain.ITunes
{
    public class ITunesLookupCollectionResult
    {
        [JsonProperty("results")]
        public IEnumerable<ITunesCollection> Results { get; set; }
    }
}
