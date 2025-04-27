using Downgrooves.Domain;
using Downgrooves.Data.Interfaces;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Downgrooves.Data.Types;

namespace Downgrooves.Data
{
    public sealed class UserDao : BaseDao, IDao<User>
    {
        private readonly IQueryable<User> _users;

        public UserDao(IOptions<AppConfig> config) : base(config)
        {
            _users = GetData(Path.Combine(BasePath, FolderNames.Users, DataFileNames.User));
        }
        public IQueryable<User> GetData(string filePath)
        {
            var users = Deserialize<IEnumerable<User>>(filePath);

            return users.AsQueryable();
        }

        public IEnumerable<User?> GetAll()
        {
            return _users;
        }

        public List<User> GetAll(Expression<Func<User, bool>> predicate)
        {
            return [.. _users.Where(predicate)];
        }

        public User? Get(int id)
        {
            return GetAll().FirstOrDefault(u => u?.Id == id);
        }

        public User? Get(string name)
        {
            return GetAll().FirstOrDefault(u => u?.UserName == name);
        }

        public User? Authenticate(string userName, string password)
        {
            return GetAll(u => String.Compare(u.UserName, userName, StringComparison.OrdinalIgnoreCase) == 0 && String.Compare(u.Password, password, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
        }

    }
}
