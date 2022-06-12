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
        private readonly IApiService _apiService;

        public ITunesLookupService(IOptions<AppConfig> config, IApiService apiService) : base(config)
        {
            _apiService = apiService;
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
            var url = _appConfig.ITunes.CollectionLookupUrl;
            var apiData = await _apiService.GetResultsFromApi(url, ApiData.ApiDataType.iTunesCollection, searchTerm);
            var results = JsonConvert.DeserializeObject<ITunesLookupResultItem[]>(apiData.Data);
            return results?
                .Where(x => x.WrapperType == "collection")
                .Where(x => !x.CollectionName.Contains("Remix"))
                ;
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm)
        {
            var url = _appConfig.ITunes.TracksLookupUrl;
            var apiData = await _apiService.GetResultsFromApi(url, ApiData.ApiDataType.iTunesTrack, searchTerm);
            var results = JsonConvert.DeserializeObject<ITunesLookupResultItem[]>(apiData.Data);
            return results?.Where(x => x.WrapperType == "track");
        }

        #endregion Apple iTunes API
    }
}