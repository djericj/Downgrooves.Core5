using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        IEnumerable<Artist> GetArtists();

        IEnumerable<Artist> GetArtistsAndReleases();
    }
}