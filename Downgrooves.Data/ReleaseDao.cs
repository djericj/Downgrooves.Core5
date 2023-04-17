using Downgrooves.Data.Interfaces;
using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Downgrooves.Data.Adapters;
using Newtonsoft.Json.Linq;

namespace Downgrooves.Data
{
    public sealed class ReleaseDao : BaseDao, IDao<Release>
    {
        private readonly IQueryable<Release> _releases;

        public ReleaseDao(IConfiguration configuration) : base(configuration)
        {
            _releases = GetData(Path.Combine(BasePath, "iTunes"));
        }

        public IQueryable<Release> GetData(string filePath)
        {
            var releases = new DirectoryInfo(filePath)
                .GetFiles("*.json")
                .Select(file => GetTrackByCollectionId(file.FullName))
                .ToList();

            return releases.AsQueryable();
        }

        public IEnumerable<Release> GetAll()
        {
            return _releases;
        }

        public List<Release> GetAll(Expression<Func<Release, bool>> predicate)
        {
            return _releases.Where(predicate).ToList();
        }

        public IEnumerable<Release> GetAll(string artist)
        {
            return GetAll().Where(r => string.Compare(r.ArtistName, artist, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public Release Get(string name)
        {
            return GetAll().FirstOrDefault(r => r.Title == name)!;
        }

        public Release Get(int id)
        {
            return GetAll().FirstOrDefault(r => r.Id == id)!;
        }
        private static Release GetTrackByCollectionId(string filePath)
        {
            if (!File.Exists(filePath))
                return null!;

            var content = JArray.Parse(ReadFile(filePath));
            var collection = content.SelectTokens("$[?(@.wrapperType =='collection')]")
                .Select(c => c.ToObject<ITunesCollection>()!).FirstOrDefault();

            var tracks = content.SelectTokens("$[?(@.wrapperType =='track')]")
                .Select(c => c.ToObject<ITunesTrack>()!).ToList();

            var releaseTracks = ReleaseAdapter.CreateTracks(tracks) as ICollection<ReleaseTrack>;

            return (collection != null ? ReleaseAdapter.CreateRelease(collection, releaseTracks) : null)!;
        }
    }
}
