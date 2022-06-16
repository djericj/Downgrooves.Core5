using Downgrooves.Model;
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

        public async Task<User> Authenticate(string userName, string password)
        {
            return await _unitOfWork.Users.Authenticate(userName, password);
        }
    }
}