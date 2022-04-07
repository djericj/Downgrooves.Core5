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
        public string Artist { get; set; }
        public string Attachment { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }

        [Key]
        [Column("mixId")]
        public int MixId { get; set; }

        public string Length { get; set; }
        public string Mp3File { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int Show { get; set; }
        public int TotalPlays { get; set; }

        [InverseProperty("Mix")]
        public ICollection<MixTrack> Tracks { get; set; }
    }
}