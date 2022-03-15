using Downgrooves.Domain;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string userName, string password);
    }
}