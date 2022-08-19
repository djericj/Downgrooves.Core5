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
        public string CollectionType { get; set; }
        public string Copyright { get; set; }

        [JsonProperty("CollectionId")]
        public new int Id { get; set; } // CollectionId
    }
}