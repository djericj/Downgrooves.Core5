using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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