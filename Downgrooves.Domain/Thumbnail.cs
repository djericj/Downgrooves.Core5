namespace Downgrooves.Domain
{
    /// <summary>
    /// An thumbnail image
    /// </summary>
    public class Thumbnail
    {
        public int Height { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int VideoId { get; set; }
        public int Width { get; set; }
    }
}