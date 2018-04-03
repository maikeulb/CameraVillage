using RolleiShop.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Services.Interfaces
{
    public interface ICartViewModelService
    {
        Task<CartViewModel> GetOrCreateCartForUser(string userName);
    }
}
