using Newtonsoft.Json;
using System;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A collection item from the iTunes API
    /// </summary>
    [Serializable]
    public class ITunesCollection : ITunesItem
    {
        public string Copyright { get; set; }

        [JsonProperty("CollectionId")]
        public override int? Id { get; set; }
    }
}