﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("releaseTrack")]
    public class ReleaseTrack
    {
        public string ArtistName { get; set; }

        [Key]
        [Column("trackId")]
        public int Id { get; set; }

        public string PreviewUrl { get; set; }
        public double Price { get; set; }

        [JsonIgnore]
        public Release Release { get; set; }

        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public int TrackTimeInMilliseconds { get; set; }
    }
}