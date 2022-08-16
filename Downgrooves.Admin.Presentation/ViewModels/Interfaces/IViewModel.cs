using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public interface IViewModel
    {
        Task Add();

        Task Update();

        Task Remove(int id);
    }
}