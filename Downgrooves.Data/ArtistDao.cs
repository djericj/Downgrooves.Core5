using Downgrooves.Data.Interfaces;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;

namespace Downgrooves.Data
{
    public sealed class ArtistDao : BaseDao, IDao<Artist>
    {
        private readonly IEnumerable<Artist>? _artists;

        public ArtistDao(IOptions<AppConfig> config) : base(config)
        {
            _artists = GetData(Path.Combine(BasePath, "artist.json"));
        }

        public IQueryable<Artist> GetData(string filePath)
        {
            var artists = Deserialize<IEnumerable<Artist>>(filePath);

            return artists.AsQueryable();
        }

        public IEnumerable<Artist?> GetAll()
        {
            return _artists ?? Array.Empty<Artist>();
        }

        public Artist? Get(int id)
        {
            return GetAll().FirstOrDefault(a => a?.Id == id);
        }

        public Artist? Get(string name)
        {
            return GetAll().FirstOrDefault(a => a?.Name == name);
        }

        
    }
}
