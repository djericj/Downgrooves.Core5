using Downgrooves.Domain;
using ITunesLoader.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ITunesLoader.Services
{
    internal class TrackService : ITrackService
    {
        private int index = 0;
        private readonly IConfiguration _configuration;
        private readonly IITunesService _iTunesService;
        private readonly ILogger<TrackService> _logger;
        public string ApiUrl { get; set; }
        public string Token { get; set; }
        public string ArtworkBasePath { get; }

        public TrackService(IConfiguration configuration, IITunesService iTunesService, ILogger<TrackService> logger)
        {
            _configuration = configuration;
            ApiUrl = _configuration.GetSection("AppConfig:ApiUrl").Value;
            Token = _configuration.GetSection("AppConfig:Token").Value;
            ArtworkBasePath = _configuration.GetSection("AppConfig:ArtworkBasePath").Value;
            _iTunesService = iTunesService;
            _logger = logger;
        }

        public void GetArtwork()
        {
            try
            {
                var client = new RestClient(ApiUrl);
                client.Authenticator = new JwtAuthenticator(Token);
                var request = new RestRequest("itunes/tracks", Method.GET);
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var response = client.Get(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var collections = JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                    if (collections != null)
                        GetArtwork(collections);
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding artwork for collections.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

            }
        }

        private void GetArtwork(ITunesCollection[] collections)
        {
            foreach (var item in collections)
                GetArtwork(item);
        }

        private void GetArtwork(ITunesCollection collection)
        {
            var fileName = collection.CollectionId.ToString();
            var imagePath = $@"{ArtworkBasePath}\tracks\{fileName}.jpg";
            if (!File.Exists(imagePath))
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded artwork {imagePath}");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                }
            }
        }

        public void AddTracks(string artistName)
        {
            IEnumerable<ITunesTrack> tracksToAdd = new List<ITunesTrack>();
            var results = _iTunesService.GetItunesJson(artistName);
            var tracks = CreateTracks(results);
            var existingTracks = GetExistingTracks();
            if (existingTracks != null && existingTracks.Count() > 0)
                tracksToAdd = tracks.Where(x => existingTracks.All(y => x.TrackId != y.TrackId));
            else
                tracksToAdd = tracks;
            var count = AddNewTracks(tracksToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} tracks added.");
            GetArtwork();
        }

        public int AddNewTracks(IEnumerable<ITunesTrack> tracks)
        {
            foreach (var track in tracks)
                AddNewTrack(track);
            return index;
        }


        private void AddNewTrack(ITunesTrack track)
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
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public IEnumerable<ITunesTrack> GetExistingTracks()
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("itunes/tracks");
            var response = client.Get(request);
            var json = response.Content;
            var tracks = JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(json);
            return tracks;
        }

        public IEnumerable<ITunesTrack> CreateTracks(IJEnumerable<JToken> tokens)
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

        private ITunesTrack CreateTrack(JToken token)
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
