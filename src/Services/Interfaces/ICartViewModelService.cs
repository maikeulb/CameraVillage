using RolleiShop.Features.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Services.Interfaces
{
    public interface ICartViewModelService
    {
        Task<GetCart.Result> GetOrCreateCartForUser(string userName);
    }
}
