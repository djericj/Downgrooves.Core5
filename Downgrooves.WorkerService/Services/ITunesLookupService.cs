using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Extensions;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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
            try
            {
                string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
                var data = await GetString(url);
                return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupCollections(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?
                .Where(x => x.WrapperType == "collection")
                .Where(x => !x.CollectionName.Contains("Remix"))
                //.Where(x => x.ReleaseDate < System.DateTime.Now)
                ;
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search?term={searchTerm}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        #endregion Apple iTunes API
    }
}