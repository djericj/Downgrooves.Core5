using Downgrooves.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ITunesLoader.Services
{
    internal static class TrackService
    {
        private static int index = 0;
        public static string ApiUrl { get; set; }
        public static string Token { get; set; }

        public static int AddNewTracks(IEnumerable<ITunesTrack> tracks)
        {
            foreach (var track in tracks)
                AddNewTrack(track);
            return index;
        }

        private static void AddNewTrack(ITunesTrack track)
        {
            try
            {
                var client = new RestClient(ApiUrl);
                client.Authenticator = new JwtAuthenticator(Token);
                var request = new RestRequest("itunes/tracks", Method.POST);
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var json = JsonConvert.SerializeObject(track, settings);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                //request.AddJsonBody(track);
                var response = client.Post(request);
                var description = $"{track.ArtistName} - {track.TrackName} ({track.TrackId})";
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    Console.WriteLine($"Added {description}");
                }
                else
                    Console.Error.WriteLine($"Error adding {description}.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public static IEnumerable<ITunesTrack> GetExistingTracks()
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("itunes/tracks");
            var response = client.Get(request);
            var json = response.Content;
            var tracks = JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(json);
            return tracks;
        }

        public static IEnumerable<ITunesTrack> CreateTracks(IJEnumerable<JToken> tokens)
        {
            var tracks = new List<ITunesTrack>();
            foreach (var item in tokens)
            {
                if (item["wrapperType"].ToString() == "track")
                {
                    var track = CreateTrack(item);
                    tracks.Add(track);
                }
            }
            return tracks;
        }

        private static ITunesTrack CreateTrack(JToken token)
        {
            return new ITunesTrack()
            {
                ArtistId = Convert.ToInt32(token["artistId"]),
                ArtistName = token["artistName"].ToString(),
                ArtistViewUrl = token["artistViewUrl"].ToString(),
                ArtworkUrl100 = token["artworkUrl100"].ToString(),
                ArtworkUrl30 = token["artworkUrl30"].ToString(),
                ArtworkUrl60 = token["artworkUrl60"].ToString(),
                CollectionCensoredName = token["collectionCensoredName"].ToString(),
                CollectionExplicitness = token["collectionExplicitness"].ToString(),
                CollectionId = Convert.ToInt32(token["collectionId"]),
                CollectionName = token["collectionName"].ToString(),
                CollectionPrice = Convert.ToDouble(token["collectionPrice"]),
                CollectionViewUrl = token["collectionViewUrl"].ToString(),
                Country = token["country"].ToString(),
                Currency = token["currency"].ToString(),
                DiscCount = Convert.ToInt32(token["discCount"]),
                DiscNumber = Convert.ToInt32(token["discNumber"]),
                ReleaseDate = Convert.ToDateTime(token["releaseDate"]),
                IsStreamable = token["isStreamable"].ToString(),
                TrackId = Convert.ToInt32(token["trackId"]),
                Kind = token["kind"].ToString(),
                PreviewUrl = token["previewUrl"].ToString(),
                PrimaryGenreName = token["primaryGenreName"].ToString(),
                TrackCensoredName = token["trackCensoredName"].ToString(),
                TrackCount = Convert.ToInt32(token["trackCount"]),
                TrackExplicitness = token["trackExplicitness"].ToString(),
                TrackName = token["trackName"].ToString(),
                TrackNumber = Convert.ToInt32(token["trackNumber"]),
                TrackPrice = Convert.ToDouble(token["trackPrice"]),
                TrackTimeMillis = Convert.ToInt32(token["trackTimeMillis"]),
                TrackViewUrl = token["trackViewUrl"].ToString(),
                WrapperType = token["wrapperType"].ToString(),
            };
        }
    }
}
