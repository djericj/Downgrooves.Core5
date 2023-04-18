using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace Downgrooves.Service
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly UserDao _dao;

        public UserService(IOptions<AppConfig> config) : base(config)
        {
            var daoFactory = new DaoFactory(config);

            _dao = daoFactory.Users as UserDao;
        }

        public User Authenticate(string userName, string password)
        {
            return _dao.Authenticate(userName, password);
        }
    }
}