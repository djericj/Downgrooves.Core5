namespace Downgrooves.Domain
{
    /// <summary>
    /// A single track in a DJ Mix
    /// </summary>
    public class MixTrack
    {
        public string Artist { get; set; }
        public int Id { get; set; }
        public string Label { get; set; }
        public int MixId { get; set; }
        public int Number { get; set; }
        public string Remix { get; set; }
        public string Title { get; set; }
    }
}