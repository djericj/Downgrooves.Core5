using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A DJ Mix
    /// </summary>
    public class Mix
    {
        public string Artist { get; set; }
        public string ArtworkUrl { get; set; }
        public string AudioUrl { get; set; }
        public string BasePath { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }
        [JsonProperty("MixId")]
        public int Id { get; set; }
        public string Length { get; set; }
        public string ShortDescription { get; set; }
        public int Show { get; set; }
        public string Title { get; set; }
        public ICollection<MixTrack> Tracks { get; set; }
    }
}