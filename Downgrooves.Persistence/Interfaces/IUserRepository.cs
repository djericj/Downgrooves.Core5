using Downgrooves.Model;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Authenticate(string userName, string password);
    }
}