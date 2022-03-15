using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> Authenticate(string userName, string password)
        {
            var user = await DowngroovesDbContext.Users
                .Where(x => x.UserName == userName && x.Password == password)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}