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

        public Mix Add(Mix mix)
        {
            try
            {
                _unitOfWork.Mixes.Add(mix);
                _unitOfWork.Complete();
                return mix;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public IEnumerable<Mix> GetMixes()
        {
            return _unitOfWork.Mixes.GetMixes();
        }

        public IEnumerable<Mix> GetMixes(PagingParameters parameters)
        {
            return _unitOfWork.Mixes.GetMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetMixesAsync()
        {
            return await Task.Run(() => GetMixes());
        }

        public async Task<IEnumerable<Mix>> GetMixesAsync(PagingParameters parameters)
        {
            return await Task.Run(() => GetMixes(parameters));
        }

        public IEnumerable<Mix> GetShowMixes()
        {
            return _unitOfWork.Mixes.GetShowMixes();
        }

        public IEnumerable<Mix> GetShowMixes(PagingParameters parameters)
        {
            return _unitOfWork.Mixes.GetShowMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetShowMixesAsync()
        {
            return await Task.Run(() => _unitOfWork.Mixes.GetShowMixes());
        }

        public async Task<IEnumerable<Mix>> GetShowMixesAsync(PagingParameters parameters)
        {
            return await Task.Run(() => _unitOfWork.Mixes.GetShowMixes(parameters));
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