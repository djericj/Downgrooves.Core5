using Downgrooves.Domain;
using Microsoft.Extensions.Configuration;
using Downgrooves.Data.Interfaces;
using System.Linq.Expressions;

namespace Downgrooves.Data
{
    public sealed class UserDao : BaseDao, IDao<User>
    {
        private readonly IQueryable<User> _users;

        public UserDao(IConfiguration configuration) : base(configuration)
        {
            _users = GetData(Path.Combine(BasePath, "Users", "user.json"));
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
            return _users.Where(predicate).ToList();
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
