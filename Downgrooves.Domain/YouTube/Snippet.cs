using System;

namespace Downgrooves.Domain.YouTube
{
    public class Snippet
    {
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public Resource ResourceId { get; set; }

        public Thumbnails Thumbnails { get; set; }

        public string Title { get; set; }
    }
}