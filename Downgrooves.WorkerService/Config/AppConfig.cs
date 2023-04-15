namespace Downgrooves.WorkerService.Config
{
    public class AppConfig
    {
        public string ApiUrl { get; set; }
        public string Token { get; set; }
        public string ArtworkBasePath { get; set; }

        public int PollInterval { get; set; }
        public ITunes ITunes { get; set; }
        public YouTube YouTube { get; set; }
    }

    public class ITunes
    {
        public int PollInterval { get; set; }
        public int LookupInterval { get; set; }
        public string CollectionSearchUrl { get; set; }
        public string TracksSearchUrl { get; set; }
        public string LookupUrl { get; set; }
    }

    public class YouTube
    {
        public int PollInterval { get; set; }
        public string ApiKey { get; set; }
    }
}