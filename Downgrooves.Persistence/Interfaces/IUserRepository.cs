using Downgrooves.Domain;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string userName, string password);
    }
}