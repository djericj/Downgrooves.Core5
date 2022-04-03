using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain.ITunes
{
    [Table("itunescollection")]
    public class ITunesCollection : ITunesResult
    {
        [Key]
        public int CollectionId { get; set; }
        public string CollectionType { get; set; }
        public string Copyright { get; set; }
        [NotMapped]
        public IEnumerable<ITunesTrack> Tracks { get; set; }
    }
}
