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
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<IMixService> _logger;

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
            return await _unitOfWork.Mixes.GetMixes();
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await _unitOfWork.Mixes.GetMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetShowMixes()
        {
            return await _unitOfWork.Mixes.GetShowMixes();
        }

        public async Task<IEnumerable<Mix>> GetShowMixes(PagingParameters parameters)
        {
            return await _unitOfWork.Mixes.GetShowMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetMixesByCategory(string category)
        {
            return await _unitOfWork.Mixes.GetMixesByCategory(category);
        }

        public async Task<IEnumerable<Mix>> GetMixesByGenre(string genre)
        {
            return await _unitOfWork.Mixes.GetMixesByGenre(genre);
        }
    }
}