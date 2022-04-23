using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the artist table
    /// </summary>
    [Table("artist")]
    public class Artist
    {
        private string description;

        [Key]
        [Column("artistId")]
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string Description 
        { 
            get => System.Net.WebUtility.HtmlDecode(description); 
            set => description = value; 
        }

        [JsonIgnore]
        [ForeignKey("ArtistId")]
        public ICollection<Release> Releases { get; set; }
    }
}