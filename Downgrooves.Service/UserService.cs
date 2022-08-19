using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public async Task<User> Authenticate(string userName, string password)
        {
            return await _unitOfWork.Users.Authenticate(userName, password);
        }
    }
}