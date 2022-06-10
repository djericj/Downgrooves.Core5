using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Extensions;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesLookupService : ApiBase, IITunesLookupService
    {
        public ITunesLookupService(IOptions<AppConfig> config) : base(config)
        {
        }

        #region Apple iTunes API

        public async Task<ITunesLookupResultItem> LookupCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data).ToObjects<ITunesLookupResultItem>("results").FirstOrDefault();
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracksCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupCollections(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=300";
            var data = await GetString(url);
            var results = JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results");
            var apiData = new ApiData();
            apiData.DataType = ApiData.ApiDataType.iTunesCollection;
            apiData.Artist = searchTerm;
            apiData.Url = url;
            apiData.Data = data;
            apiData.LastUpdate = DateTime.Now;
            await AddApiData(apiData);
            return results?
                .Where(x => x.WrapperType == "collection")
                .Where(x => !x.CollectionName.Contains("Remix"))
                ;
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search?term={searchTerm}&entity=song&limit=300";
            var data = await GetString(url);
            var results = JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results");
            var apiData = new ApiData();
            apiData.DataType = ApiData.ApiDataType.iTunesTrack;
            apiData.Artist = searchTerm;
            apiData.Url = url;
            apiData.Data = data;
            apiData.LastUpdate = DateTime.Now;
            await AddApiData(apiData);
            return results?.Where(x => x.WrapperType == "track");
        }

        public async Task<ApiData> AddApiData(ApiData data)
        {
            var content = "";
            var response = await ApiPost("itunes/data", data);
            if (response.IsSuccessful)
                content = response.Content;

            if (!string.IsNullOrEmpty(content))
                return JsonConvert.DeserializeObject<ApiData>(content);
            return null;
        }

        #endregion Apple iTunes API
    }
}