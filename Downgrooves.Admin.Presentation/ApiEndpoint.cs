namespace Downgrooves.Admin.Presentation
{
    public class ApiEndpoint
    {
        public static string Artists = new("/artists");
        public static string ArtistReleases = new("/artists/releases");
        public static string ITunesCollection = new("/itunes/collection");
        public static string ITunesCollections = new("/itunes/collections");
        public static string ITunesTrack = new("/itunes/track");
        public static string ITunesTracks = new("/itunes/tracks");
        public static string Genres = new("/genres");
        public static string Logs = new("/logs");
        public static string Mix = new("/mix");
        public static string MixTrack = new("/mix/track");
        public static string MixTracks = new("/mix/tracks");
        public static string Mixes = new("/mixes");
        public static string Release = new("/release");
        public static string ReleaseTrack = new("/release/track");
        public static string Releases = new("/releases");
        public static string ReleaseTracks = new("/release/tracks");
        public static string Video = new("/video");
        public static string Videos = new("/videos");

        public ApiEndpoint(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}