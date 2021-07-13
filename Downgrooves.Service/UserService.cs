using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public User Authenticate(string userName, string password)
        {
            try
            {
                return _unitOfWork.Users.Authenticate(userName, password);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.UserService.Authenticate {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<User> AuthenticateAsync(string userName, string password)
        {
            return await Task.Run(() => Authenticate(userName, password));
        }
    }
}