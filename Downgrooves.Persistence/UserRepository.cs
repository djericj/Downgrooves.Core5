using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public UserRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public User Authenticate(string userName, string password)
        {
            var user = DowngroovesDbContext.Users
                .Where(x => string.Compare(x.UserName, userName, true) == 0 && string.Compare(x.Password, password, true) == 0)
                .FirstOrDefault();
            return user;
        }
    }
}