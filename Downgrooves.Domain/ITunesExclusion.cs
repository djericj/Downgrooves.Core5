using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("exclusions")]
    public class ITunesExclusion
    {
        public int CollectionId { get; set; }
        public int TrackId { get; set; }
    }
}
