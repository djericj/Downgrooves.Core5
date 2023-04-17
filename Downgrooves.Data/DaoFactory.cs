using Downgrooves.Data.Interfaces;
using Downgrooves.Domain;
using Microsoft.Extensions.Configuration;

namespace Downgrooves.Data
{
    public class DaoFactory
    {
        public DaoFactory(IConfiguration configuration)
        {
            Artists = new ArtistDao(configuration);
            Releases = new ReleaseDao(configuration);
        }

        public IDao<Artist> Artists { get; set; }
        public IDao<Release> Releases { get; set; }
    }
}
