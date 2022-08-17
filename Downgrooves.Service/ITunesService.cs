using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public ITunesService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<ITunesCollection> AddCollection(ITunesCollection item)
        {
            await _unitOfWork.ITunesCollection.AddAsync(item);
            await _unitOfWork.CompleteAsync();
            return item;
        }

        public async Task<IEnumerable<ITunesCollection>> AddCollections(IEnumerable<ITunesCollection> items)
        {
            await _unitOfWork.ITunesCollection.AddRangeAsync(items);
            await _unitOfWork.CompleteAsync();
            return items;
        }

        public async Task<ITunesTrack> AddTrack(ITunesTrack item)
        {
            await _unitOfWork.ITunesTrack.AddAsync(item);
            await _unitOfWork.CompleteAsync();
            return item;
        }

        public async Task<IEnumerable<ITunesTrack>> AddTracks(IEnumerable<ITunesTrack> items)
        {
            await _unitOfWork.ITunesTrack.AddRangeAsync(items);
            await _unitOfWork.CompleteAsync();
            return items;
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null)
        {
            if (artistName != null)
                return await _unitOfWork.ITunesCollection.FindAsync(x => x.ArtistName.Contains(artistName));
            else
                return await _unitOfWork.ITunesCollection.GetAllAsync();
        }

        public async Task<ITunesCollection> GetCollection(int id)
        {
            return await _unitOfWork.ITunesCollection.GetAsync(id);
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null)
        {
            if (artistName != null)
                return await _unitOfWork.ITunesTrack.FindAsync(x => x.TrackCensoredName.Contains(artistName));
            else
                return await _unitOfWork.ITunesTrack.GetAllAsync();
        }

        public async Task<ITunesTrack> GetTrack(int id)
        {
            return await _unitOfWork.ITunesTrack.GetAsync(id);
        }

        public async Task<IEnumerable<ITunesExclusion>> GetExclusions()
        {
            return await _unitOfWork.ITunesExclusion.GetAllAsync();
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> Lookup(int Id)
        {
            var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("country", "us", ParameterType.UrlSegment);
            request.AddParameter("id", Id);
            request.AddParameter("entity", "musicArtist,musicTrack,album,mix,song");
            request.AddParameter("media", "music");
            var response = await client.ExecuteAsync<ITunesLookupResult>(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(response.Content);
                return lookupResult.Results;
            }

            return null;
        }

        public async Task RemoveCollection(int id)
        {
            var collection = await _unitOfWork.ITunesCollection.GetAsync(id);
            await _unitOfWork.ITunesCollection.Remove(collection);
            await _unitOfWork.CompleteAsync();
            return;
        }

        public async Task RemoveTrack(int id)
        {
            var track = await _unitOfWork.ITunesTrack.GetAsync(id);
            await _unitOfWork.ITunesTrack.Remove(track);
            await _unitOfWork.CompleteAsync();
            return;
        }

        public async Task RemoveCollections(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                await RemoveCollection(item);
        }

        public async Task RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                await RemoveTrack(item);
        }

        public async Task<ITunesCollection> UpdateCollection(ITunesCollection item)
        {
            _unitOfWork.ITunesCollection.UpdateState(item);
            await _unitOfWork.CompleteAsync();
            return item;
        }

        public async Task<ITunesTrack> UpdateTrack(ITunesTrack item)
        {
            _unitOfWork.ITunesTrack.UpdateState(item);
            await _unitOfWork.CompleteAsync();
            return item;
        }

        public async Task<IEnumerable<ITunesCollection>> UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            foreach (var item in items)
                await UpdateCollection(item);
            return items;
        }

        public async Task<IEnumerable<ITunesTrack>> UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            foreach (var item in items)
                await UpdateTrack(item);
            return items;
        }
    }
}