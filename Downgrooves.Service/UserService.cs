using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Authenticate(string userName, string password)
        {
            return _unitOfWork.Users.Authenticate(userName, password);
        }

        public async Task<User> AuthenticateAsync(string userName, string password)
        {
            return await Task.Run(() => Authenticate(userName, password));
        }
    }
}