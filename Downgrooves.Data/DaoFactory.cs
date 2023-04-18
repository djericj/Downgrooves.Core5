using Downgrooves.Data.Interfaces;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;

namespace Downgrooves.Data
{
    public class DaoFactory
    {
        public DaoFactory(IOptions<AppConfig> config)
        {
            Artists = new ArtistDao(config);
            Genres = new GenreDao(config);
            Mixes = new MixDao(config);
            Releases = new ReleaseDao(config);
            Users = new UserDao(config);
            Videos = new VideoDao(config);
        }

        public IDao<Artist> Artists { get; set; }
        public IDao<Genre> Genres { get; set; }
        public IDao<Mix> Mixes { get; set; }
        public IDao<Release> Releases { get; set; }
        public IDao<User> Users { get; set; }
        public IDao<Video> Videos { get; set; }
    }
}
