using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class MixService : IMixService
    {
        private readonly ILogger<IMixService> _logger;
        private IUnitOfWork _unitOfWork;

        public MixService(IUnitOfWork unitOfWork, ILogger<IMixService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Mix> Add(Mix mix)
        {
            try
            {
                _unitOfWork.Mixes.Add(mix);
                await _unitOfWork.CompleteAsync();
                return mix;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Mix>> GetMixes()
        {
            return await _unitOfWork.Mixes.GetAllAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await _unitOfWork.Mixes.GetMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetMixesByCategory(string category)
        {
            return await _unitOfWork.Mixes.FindAsync(x => x.Category.ToUpper().Equals(category.ToUpper()));
        }

        public async Task<IEnumerable<Mix>> GetMixesByGenre(string genre)
        {
            return await _unitOfWork.Mixes.FindAsync(x => x.Genre.Name == genre);
        }

        public async Task<Mix> GetMix(int id)
        {
            return await _unitOfWork.Mixes.GetMix(id);
        }
    }
}