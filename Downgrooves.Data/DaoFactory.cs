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
            Genres = new GenreDao(configuration);
            Mixes = new MixDao(configuration);
            Releases = new ReleaseDao(configuration);
            Users = new UserDao(configuration);
            Videos = new VideoDao(configuration);
        }

        public IDao<Artist> Artists { get; set; }
        public IDao<Genre> Genres { get; set; }
        public IDao<Mix> Mixes { get; set; }
        public IDao<Release> Releases { get; set; }
        public IDao<User> Users { get; set; }
        public IDao<Video> Videos { get; set; }
    }
}
