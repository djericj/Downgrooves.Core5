using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IArtistService
    {
        IEnumerable<Artist> GetArtists();

        Artist GetArtist(int id);

        Artist GetArtist(string name);
    }
}