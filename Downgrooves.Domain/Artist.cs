using System.Collections.Generic;

namespace Downgrooves.Domain
{
    /// <summary>
    /// A music artist.
    /// </summary>
    public class Artist
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Release> Releases { get; set; }
    }
}