using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        [InverseProperty("Genre")]
        public ICollection<Mix> Mixes { get; set; }
    }
}