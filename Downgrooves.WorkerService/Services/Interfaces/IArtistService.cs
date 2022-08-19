using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IArtistService
    {
        IEnumerable<Artist> GetArtists();
    }
}