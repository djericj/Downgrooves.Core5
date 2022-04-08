using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the release table
    /// </summary>
    [Table("release")]
    public class Release
    {
        private string _artworkUrl;
        private string _basePath;

        public int CollectionId { get; set; }

        public Artist Artist { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }
        public string ArtistViewUrl { get; set; }

        public string ArtworkUrl { get => $"{_basePath}/images/artwork/collections/{_artworkUrl}"; set => _artworkUrl = value; }

        [NotMapped]
        [JsonIgnore]
        public string BasePath { get => _basePath; set => _basePath = value; }

        public string BuyUrl { get; set; }
        public string Copyright { get; set; }
        public string Country { get; set; }
        public int DiscCount { get; set; }
        public int DiscNumber { get; set; }
        public string Genre { get; set; }

        [Key]
        [Column("releaseId")]
        public int Id { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsRemix { get; set; }
        public string PreviewUrl { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Title { get; set; }

        [InverseProperty("Release")]
        public ICollection<ReleaseTrack> Tracks { get; set; }

        public int VendorId { get; set; }
    }
}