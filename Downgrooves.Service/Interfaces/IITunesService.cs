using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        Task<ITunesLookupResultItem> Add(ITunesLookupResultItem item);

        Task<IEnumerable<ITunesLookupResultItem>> AddRange(IEnumerable<ITunesLookupResultItem> items);

        Task<IEnumerable<ITunesLookupResultItem>> GetItems();

        Task<IEnumerable<ITunesLookupResultItem>> Lookup(int Id);
    }
}