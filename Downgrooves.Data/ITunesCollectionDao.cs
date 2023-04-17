using Downgrooves.Domain.ITunes;

namespace Downgrooves.Data
{
    public class ITunesCollectionDao
    {
        public IEnumerable<ITunesCollection> GetCollections()
        {
            return new List<ITunesCollection>();
        }
    }
}