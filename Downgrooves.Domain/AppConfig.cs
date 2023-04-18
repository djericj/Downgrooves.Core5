namespace Downgrooves.Domain
{
    public class AppConfig
    {
        public string[] WebAppUrls { get; set; }
        public string CdnUrl { get; set; }
        public string MediaBasePath { get; set; }
        public string JsonDataBasePath { get; set; }
        public string ITunesLookupUrl { get; set; }
        public Exclusions Exclusions { get; set; }
    }

    public class Exclusions
    {
        public int[] CollectionIds { get; set; }
        public string[] Keywords { get; set; }
    }
}
