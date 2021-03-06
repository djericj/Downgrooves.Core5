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
        public string CollectionLookupUrl { get; set; }
        public string TracksLookupUrl { get; set; }
    }

    public class YouTube
    {
        public int PollInterval { get; set; }
        public string ApiKey { get; set; }
    }
}