using Newtonsoft.Json;
using System;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A track item from the iTunes API
    /// </summary>
    [Serializable]
    public class ITunesTrack : ITunesItem
    {
        public int CollectionId { get; set; }
        public int DiscCount { get; set; }
        public int DiscNumber { get; set; }

        [JsonProperty("TrackId")]
        public override int Id { get; set; } // TrackId

        public string IsStreamable { get; set; }
        public string Kind { get; set; }
        public string PreviewUrl { get; set; }
        public double? TrackPrice { get; set; }
        public string TrackCensoredName { get; set; }
        public string TrackExplicitness { get; set; }
        public string TrackName { get; set; }
        public int TrackNumber { get; set; }
        public int TrackTimeMillis { get; set; }
        public string TrackViewUrl { get; set; }
    }
}