using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class MixService : IMixService
    {
        private IUnitOfWork _unitOfWork;

        public MixService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Mix> GetMixes()
        {
            return _unitOfWork.Mixes.GetAll();
        }

        public async Task<IEnumerable<Mix>> GetMixesAsync()
        {
            return await Task.Run(() => GetMixes());
        }

        public IEnumerable<Mix> GetMixesByCategory(string category)
        {
            return _unitOfWork.Mixes.GetMixesByCategory(category);
        }

        public IEnumerable<Mix> GetMixesByGenre(string genre)
        {
            return _unitOfWork.Mixes.GetMixesByGenre(genre);
        }
    }
}