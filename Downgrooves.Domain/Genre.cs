using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A row in the genre table
    /// </summary>
    [Table("genre")]
    public class Genre
    {
        public int GenreId { get; set; }

        [JsonIgnore]
        public ICollection<Mix> Mixes { get; set; }

        public string Name { get; set; }
    }
}