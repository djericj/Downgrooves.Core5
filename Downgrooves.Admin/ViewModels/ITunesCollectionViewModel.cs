using Downgrooves.Admin.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Admin.ViewModels
{
    public class ITunesCollectionViewModel : ITunesViewModel
    {
        protected IApiService<ITunesCollection> _service;

        public ITunesCollectionViewModel(IApiService<ITunesCollection> service)
        {
            _service = service;
        }

        [Required(ErrorMessage = null)]
        public string CollectionType { get; set; }

        [Required(ErrorMessage = null)]
        public string Copyright { get; set; }

        public async Task AddCollection()
        {
            var collection = CreateCollection(this);
            MapToViewModel(await _service.Add(collection, ApiEndpoint.ITunesCollection));
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections()
        {
            return await _service.GetAll(ApiEndpoint.ITunesCollections);
        }

        public async Task GetCollection(int id)
        {
            MapToViewModel(await _service.Get(id, ApiEndpoint.ITunesCollection));
        }

        public async Task UpdateCollection()
        {
            var collection = CreateCollection(this);
            MapToViewModel(await _service.Update(collection, ApiEndpoint.ITunesCollection));
        }

        public async Task Remove(int id)
        {
            await _service.Remove(id, ApiEndpoint.ITunesCollection);
        }

        private ITunesCollection CreateCollection(ITunesCollectionViewModel viewModel)
        {
            return new ITunesCollection()
            {
                ArtistId = viewModel.ArtistId,
                ArtistName = viewModel.ArtistName,
                ArtistViewUrl = viewModel.ArtistViewUrl,
                ArtworkUrl100 = viewModel.ArtworkUrl100,
                ArtworkUrl60 = viewModel.ArtworkUrl60,
                CollectionCensoredName = viewModel.CollectionCensoredName,
                CollectionExplicitness = viewModel.CollectionExplicitness,
                Id = viewModel.CollectionId,
                CollectionPrice = viewModel.CollectionPrice,
                CollectionType = viewModel.CollectionType,
                CollectionViewUrl = viewModel.CollectionViewUrl,
                Copyright = viewModel.Copyright,
                Country = viewModel.Country,
                Currency = viewModel.Currency,
                TrackCount = viewModel.TrackCount,
                PrimaryGenreName = viewModel.PrimaryGenreName,
                ReleaseDate = viewModel.ReleaseDate,
                WrapperType = viewModel.WrapperType,
            };
        }

        private void MapToViewModel(ITunesCollection collection)
        {
            ArtistId = collection.ArtistId;
            ArtistName = collection.ArtistName;
            ArtistViewUrl = collection.ArtistViewUrl;
            ArtworkUrl100 = collection.ArtworkUrl100;
            ArtworkUrl60 = collection.ArtworkUrl60;
            CollectionCensoredName = collection.CollectionCensoredName;
            CollectionExplicitness = collection.CollectionExplicitness;
            CollectionId = collection.Id;
            CollectionPrice = collection.CollectionPrice.Value;
            CollectionType = collection.CollectionType;
            CollectionViewUrl = collection.CollectionViewUrl;
            Copyright = collection.Copyright;
            Country = collection.Country;
            Currency = collection.Currency;
            TrackCount = collection.TrackCount;
            Id = collection.Id;
            PrimaryGenreName = collection.PrimaryGenreName;
            ReleaseDate = collection.ReleaseDate.Value;
            WrapperType = collection.WrapperType;
        }
    }
}