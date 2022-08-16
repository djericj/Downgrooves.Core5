namespace Downgrooves.Domain.ITunes
{
    /// <summary>
    /// A collection item from the iTunes API
    /// </summary>
    public class ITunesCollection : ITunesItem
    {
        public string CollectionType { get; set; }
        public string Copyright { get; set; }
        public int Id { get; set; } // CollectionId
        public string WrapperType { get; set; }
    }
}