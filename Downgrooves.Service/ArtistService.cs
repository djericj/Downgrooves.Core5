using Downgrooves.Model;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ArtistService : IArtistService
    {
        private IUnitOfWork _unitOfWork;

        public ArtistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Artist>> GetArtists() => await _unitOfWork.Artists.GetArtists();

        public async Task<IEnumerable<Artist>> GetArtistsAndReleases() => await _unitOfWork.Artists.GetArtistsAndReleases();
    }
}