using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    [Table("genre")]
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Mix> Mixes { get; set; }
    }
}