using Downgrooves.Data.Interfaces;
using Downgrooves.Data.Types;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;

namespace Downgrooves.Data
{
    public sealed class GenreDao : BaseDao, IDao<Genre>
    {
        private readonly IEnumerable<Genre>? _genres;

        public GenreDao(IOptions<AppConfig> config) : base(config)
        {
            _genres = GetData(Path.Combine(BasePath, DataFileNames.Genre));
        }

        public IQueryable<Genre> GetData(string filePath)
        {
            var genres = Deserialize<IEnumerable<Genre>>(filePath);

            return genres.AsQueryable();
        }

        public IEnumerable<Genre?> GetAll()
        {
            return _genres ?? Array.Empty<Genre>();
        }

        public Genre? Get(int id)
        {
            return GetAll().FirstOrDefault(g => g?.Id == id);
        }

        public Genre? Get(string name)
        {
            return GetAll().FirstOrDefault(g => g?.Name == name);
        }
    }
}
