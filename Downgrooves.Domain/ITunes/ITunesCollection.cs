using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A row in the iTunesCollection table.
    /// </summary>
    [Table("iTunesCollection")]
    public class ITunesCollection : ITunesItem
    {
        [Key]
        public int CollectionId { get; set; }

        public string CollectionType { get; set; }
        public string Copyright { get; set; }
    }
}