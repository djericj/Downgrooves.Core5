using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Downgrooves.Domain.ITunes;
using Newtonsoft.Json;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the release table
    /// </summary>
    [Table("release")]
    public class Release
    {
        public int SourceSystemId { get; set; }
        public string ArtistName { get; set; }
        public string ArtistViewUrl { get; set; }
        public string ArtworkUrl { get; set; }
        public string BuyUrl { get; set; }
        public string Copyright { get; set; }
        public string Country { get; set; }
        public int DiscCount { get; set; }
        public int DiscNumber { get; set; }
        public string Genre { get; set; }

        [Key]
        [Column("releaseId")]
        public int Id { get; set; }

        [NotMapped]
        public bool IsOriginal => ArtistName?.Contains("Downgrooves", StringComparison.OrdinalIgnoreCase) ?? false;

        [NotMapped]
        public bool IsRemix => Title?.Contains("Downgrooves", StringComparison.OrdinalIgnoreCase) ?? false;

        public string PreviewUrl { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Title { get; set; }

        [InverseProperty("Release")]
        public ICollection<ReleaseTrack> Tracks { get; set; }

        public int VendorId { get; set; }
    }
}