using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the mix table
    /// </summary>
    [Table("mix")]
    public class Mix
    {
        private string _audioUrl;
        private string _artworkUrl;
        private string _basePath;

        public string Artist { get; set; }

        public string ArtworkUrl { get => $"{_basePath}/images/mixes/{_artworkUrl}"; set => _artworkUrl = value; }

        [NotMapped]
        [JsonIgnore]
        public string BasePath { get => _basePath; set => _basePath = value; }

        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }

        [Key]
        [Column("mixId")]
        public int MixId { get; set; }

        public string Length { get; set; }

        public string AudioUrl
        {
            get => $"{_basePath}/mp3/{_audioUrl}";
            set => _audioUrl = value;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int Show { get; set; }
        public int TotalPlays { get; set; }

        [InverseProperty("Mix")]
        public ICollection<MixTrack> Tracks { get; set; }
    }
}