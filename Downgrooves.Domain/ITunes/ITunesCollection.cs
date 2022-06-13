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
        [Column("iTunesCollectionId")]
        public int Id { get; set; }

        public int CollectionId { get; set; }

        public string CollectionType { get; set; }
        public string Copyright { get; set; }
        public string WrapperType { get; set; }
    }
}