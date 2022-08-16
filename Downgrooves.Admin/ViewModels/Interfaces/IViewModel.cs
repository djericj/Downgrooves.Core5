using System.Threading.Tasks;

namespace Downgrooves.Admin.ViewModels.Interfaces
{
    public interface IViewModel
    {
        Task Add();

        Task Update();

        Task Remove(int id);
    }
}