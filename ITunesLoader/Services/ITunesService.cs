using ITunesLoader.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ITunesLoader.Services
{
    internal class ITunesService : IITunesService
    {
        public IJEnumerable<JToken> GetItunesJson(string searchTerm)
        {
            string data = null;
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            JObject o = JObject.Parse(data);
            var results = o.SelectTokens("results").Children();
            return results;
        }
    }
}
