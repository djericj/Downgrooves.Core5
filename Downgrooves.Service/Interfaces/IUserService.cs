using Downgrooves.Domain;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string userName, string password);

        Task<User> AuthenticateAsync(string userName, string password);
    }
}