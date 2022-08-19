using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ArtistService : ServiceBase, IArtistService
    {
        public ArtistService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public async Task<IEnumerable<Artist>> GetArtists() => await _unitOfWork.Artists.GetArtists();

        public async Task<IEnumerable<Artist>> GetArtistsAndReleases() => await _unitOfWork.Artists.GetArtistsAndReleases();
    }
}