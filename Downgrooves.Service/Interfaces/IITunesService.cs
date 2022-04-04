using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        Task<IEnumerable<Release>> Lookup(int Id);
    }
}