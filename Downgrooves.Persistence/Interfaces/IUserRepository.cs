using Downgrooves.Domain;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string userName, string password);
    }
}