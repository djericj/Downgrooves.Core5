using Newtonsoft.Json;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A music genre
    /// </summary>
    public class Genre
    {
        [JsonProperty("GenreId")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}