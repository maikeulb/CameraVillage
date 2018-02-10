using RolleiShop.Features.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App.Interfaces
{
    public interface ICartViewModelService
    {
        Task<CartViewModel> GetOrCreateCartForUser(string userName);
    }
}
