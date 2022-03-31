using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class CollectionService : ICollectionService
    {
        private int index = 0;
        private readonly AppConfig _appConfig;
        private readonly IApiClientService _apiClientService;
        private readonly ILogger<CollectionService> _logger;
        public string ApiUrl { get; }
        public string Token { get; }
        public string ArtworkBasePath { get; }

        public CollectionService(IOptions<AppConfig> config, IApiClientService apiClientService, ILogger<CollectionService> logger)
        {
            _appConfig = config.Value;  
            ApiUrl = _appConfig.ApiUrl;
            Token = _appConfig.Token;
            ArtworkBasePath = _appConfig.ArtworkBasePath;
            _apiClientService = apiClientService;
            _logger = logger;
        }

        public void GetArtwork()
        {
            try
            {
                var client = new RestClient(ApiUrl);
                client.Authenticator = new JwtAuthenticator(Token);
                var request = new RestRequest("itunes/collections", Method.GET);
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
            var imagePath = Path.Combine(ArtworkBasePath, "collections", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded artwork {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);

                    }

                }
            }
        }

        public void AddCollections(string artistName)
        {
            IEnumerable<ITunesCollection> collectionsToAdd = new List<ITunesCollection>();
            var results = _apiClientService.GetItunesJson(artistName);
            var collections = CreateCollections(results);
            var existingCollections = GetExistingCollections();
            if (existingCollections != null && existingCollections.Count() > 0)
                collectionsToAdd = collections.Where(x => existingCollections.All(y => x.CollectionId != y.CollectionId));
            else
                collectionsToAdd = collections;
            var count = AddNewCollections(collectionsToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} collections added.");
            GetArtwork();
        }

        public int AddNewCollections(IEnumerable<ITunesCollection> collections)
        {
            foreach (var track in collections)
                AddNewCollection(track);
            return index;
        }

        private void AddNewCollection(ITunesCollection collection)
        {
            try
            {
                var client = new RestClient(ApiUrl);
                client.Authenticator = new JwtAuthenticator(Token);
                var request = new RestRequest("itunes/collections", Method.POST);
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var json = JsonConvert.SerializeObject(collection, settings);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var description = $"{collection.ArtistName} - {collection.CollectionName} ({collection.CollectionId})";
                var response = client.Post(request);
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

        public IEnumerable<ITunesCollection> GetExistingCollections()
        {
            IEnumerable<ITunesCollection> collections = null;
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("itunes/collections");
            var response = client.Get(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = response.Content;
                collections = JsonConvert.DeserializeObject<IEnumerable<ITunesCollection>>(json);
            }
            else
            {
                _logger.LogError($"Error getting existing collections.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            return collections;
        }

        public IEnumerable<ITunesCollection> CreateCollections(IJEnumerable<JToken> tokens)
        {
            var collections = new List<ITunesCollection>();
            foreach (var item in tokens)
            {
                if (item["wrapperType"].ToString() == "collection")
                {
                    var track = CreateCollection(item);
                    collections.Add(track);
                }
            }
            return collections;
        }

        private ITunesCollection CreateCollection(JToken token)
        {
            return new ITunesCollection()
            {
                ArtistId = Convert.ToInt32(token["artistId"]),
                ArtistName = token["artistName"].ToString(),
                ArtistViewUrl = token["artistViewUrl"].ToString(),
                ArtworkUrl100 = token["artworkUrl100"].ToString(),
                ArtworkUrl60 = token["artworkUrl60"].ToString(),
                CollectionCensoredName = token["collectionCensoredName"].ToString(),
                CollectionExplicitness = token["collectionExplicitness"].ToString(),
                CollectionId = Convert.ToInt32(token["collectionId"]),
                CollectionName = token["collectionName"].ToString(),
                CollectionPrice = Convert.ToDouble(token["collectionPrice"]),
                CollectionType = token["collectionType"].ToString(),
                CollectionViewUrl = token["collectionViewUrl"].ToString(),
                Copyright = token["copyright"].ToString(),
                Country = token["country"].ToString(),
                Currency = token["currency"].ToString(),
                ReleaseDate = Convert.ToDateTime(token["releaseDate"]),
                PrimaryGenreName = token["primaryGenreName"].ToString(),
                TrackCount = Convert.ToInt32(token["trackCount"]),
                WrapperType = token["wrapperType"].ToString(),
            };
        }
    }
}
