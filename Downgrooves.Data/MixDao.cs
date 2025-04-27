using Downgrooves.Data.Interfaces;
using Downgrooves.Data.Types;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Downgrooves.Data
{
    public sealed class MixDao : BaseDao, IDao<Mix>
    {
        private readonly IQueryable<Mix> _mixes;
        private readonly IQueryable<MixTrack> _mixTracks;

        public MixDao(IOptions<AppConfig> config) : base(config)
        {
            _mixes = GetData(Path.Combine(BasePath, DataFileNames.Mixes));
            _mixTracks = GetMixTracks(Path.Combine(BasePath, DataFileNames.MixTracks));
        }

        public IQueryable<Mix> GetData(string filePath)
        {
            var mixes = Deserialize<IEnumerable<Mix>>(filePath);

            return mixes.AsQueryable();
        }

        public IQueryable<MixTrack> GetMixTracks(string filePath)
        {
            var mixTracks = Deserialize<IEnumerable<MixTrack>>(filePath);

            return mixTracks.AsQueryable();
        }

        public IEnumerable<Mix?> GetAll()
        {
            return _mixes;
        }

        public List<Mix> GetAll(Expression<Func<Mix, bool>> predicate)
        {
            return _mixes.Where(predicate).ToList();
        }

        public Mix? Get(int id)
        {
            var mix = GetAll().FirstOrDefault(m => m?.Id == id);
            if (mix != null)
                mix.Tracks = GetMixTracks(mix.Id);

            return mix;

        }

        public Mix? Get(string name)
        {
            var mix = GetAll().FirstOrDefault(m => m?.Title == name);
            if (mix != null)
                mix.Tracks = GetMixTracks(mix.Id);

            return mix;
        }

        private ICollection<MixTrack> GetMixTracks(int mixId)
        {
            return _mixTracks.Where(t => t.MixId == mixId).OrderBy(t => t.Number).ToList();
        }
    }
}
