using Downgrooves.Domain;

namespace Downgrooves.Data
{
    internal class ArtistDao : BaseDao
    {
        private readonly List<Artist>? _artists;

        public ArtistDao(string filePath) : base(filePath)
        {
            _filePath = filePath;
            _artists = GetData<List<Artist>>();
        }

        public IEnumerable<Artist>? GetArtists()
        {
            return _artists;
        }

        public Artist? GetArtist(int id)
        {
            return _artists?.FirstOrDefault(a => a.Id == id);
        }

        public Artist? GetArtist(string name)
        {
            return _artists?.FirstOrDefault(a => a.Name == name);
        }
    }
}
