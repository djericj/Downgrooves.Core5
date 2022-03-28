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
    public static  class CollectionService
    {
        private static int index = 0;
        public static string ApiUrl { get; set; }
        public static string Token { get; set; }

        public static int AddNewCollections(IEnumerable<ITunesCollection> collections)
        {
            foreach (var track in collections)
                AddNewCollection(track);
            return index;
        }

        private static void AddNewCollection(ITunesCollection collection)
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
                    Console.WriteLine($"Added {description}");
                }
                else
                    Console.Error.WriteLine($"Error adding {description}.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                
            }

        }

        public static IEnumerable<ITunesCollection> GetExistingCollections()
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
                Console.Error.WriteLine($"Error getting existing collections.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            return collections;
        }

        public static IEnumerable<ITunesCollection> CreateCollections(IJEnumerable<JToken> tokens)
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

        private static ITunesCollection CreateCollection(JToken token)
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
