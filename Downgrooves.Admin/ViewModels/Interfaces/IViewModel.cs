using System.Threading.Tasks;

namespace Downgrooves.Admin.ViewModels.Interfaces
{
    public interface IViewModel
    {
        void Add();

        void Update();

        void Remove(int id);
    }
}