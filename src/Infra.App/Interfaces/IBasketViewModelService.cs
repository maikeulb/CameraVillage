using RolleiShop.Features.Basket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App.Interfaces
{
    public interface IBasketViewModelService
    {
        Task<BasketViewModel> GetOrCreateBasketForUser(string userName);
    }
}
