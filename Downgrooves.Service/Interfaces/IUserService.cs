using Downgrooves.Domain;

namespace Downgrooves.Service.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string userName, string password);
    }
}