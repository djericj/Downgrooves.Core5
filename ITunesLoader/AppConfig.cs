namespace ITunesLoader
{
    public class AppConfig
    {
        public string ApiCredentials { get; set; }
        public string ApiUrl { get; set; }

        public string UserName
        {
            get { return ApiCredentials?.Split(":")[0]; }
        }

        public string Password
        {
            get { return ApiCredentials?.Split(":")[1]; }
        }
    }
}