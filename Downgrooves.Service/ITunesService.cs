using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class ITunesService : ServiceBase, IITunesService
    {
        public ITunesService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public ITunesCollection AddCollection(ITunesCollection item)
        {
            _unitOfWork.ITunesCollection.Add(item);
            _unitOfWork.Complete();
            return item;
        }

        public IEnumerable<ITunesCollection> AddCollections(IEnumerable<ITunesCollection> items)
        {
            _unitOfWork.ITunesCollection.AddRange(items);
            _unitOfWork.Complete();
            return items;
        }

        public ITunesTrack AddTrack(ITunesTrack item)
        {
            _unitOfWork.ITunesTrack.Add(item);
            _unitOfWork.Complete();
            return item;
        }

        public IEnumerable<ITunesTrack> AddTracks(IEnumerable<ITunesTrack> items)
        {
            _unitOfWork.ITunesTrack.AddRange(items);
            _unitOfWork.Complete();
            return items;
        }

        public IEnumerable<ITunesCollection> GetCollections(string artistName = null)
        {
            if (artistName != null)
                return _unitOfWork.ITunesCollection.Find(x => x.ArtistName.Contains(artistName));
            else
                return _unitOfWork.ITunesCollection.GetAll();
        }

        public ITunesCollection GetCollection(int id)
        {
            return _unitOfWork.ITunesCollection.Get(id);
        }

        public IEnumerable<ITunesTrack> GetTracks(string artistName = null)
        {
            if (artistName != null)
                return _unitOfWork.ITunesTrack.Find(x => x.TrackCensoredName.Contains(artistName));
            else
                return _unitOfWork.ITunesTrack.GetAll();
        }

        public ITunesTrack GetTrack(int id)
        {
            return _unitOfWork.ITunesTrack.Get(id);
        }

        public IEnumerable<ITunesExclusion> GetExclusions()
        {
            return _unitOfWork.ITunesExclusion.GetAll();
        }

        public IEnumerable<ITunesLookupResultItem> Lookup(int Id)
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
            var response = client.ExecuteAsync<ITunesLookupResult>(request).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(response.Content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(response.Content);
                return lookupResult.Results;
            }

            return null;
        }

        public void RemoveCollection(int id)
        {
            var collection = _unitOfWork.ITunesCollection.Get(id);
            _unitOfWork.ITunesCollection.Remove(collection);
            _unitOfWork.Complete();
        }

        public void RemoveTrack(int id)
        {
            var track = _unitOfWork.ITunesTrack.Get(id);
            _unitOfWork.ITunesTrack.Remove(track);
            _unitOfWork.Complete();
        }

        public void RemoveCollections(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                RemoveCollection(item);
        }

        public void RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                RemoveTrack(item);
        }

        public ITunesCollection UpdateCollection(ITunesCollection item)
        {
            _unitOfWork.ITunesCollection.UpdateState(item);
            _unitOfWork.Complete();
            return item;
        }

        public ITunesTrack UpdateTrack(ITunesTrack item)
        {
            _unitOfWork.ITunesTrack.UpdateState(item);
            _unitOfWork.Complete();
            return item;
        }

        public IEnumerable<ITunesCollection> UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            foreach (var item in items)
                UpdateCollection(item);
            return items;
        }

        public IEnumerable<ITunesTrack> UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            foreach (var item in items)
                UpdateTrack(item);
            return items;
        }
    }
}