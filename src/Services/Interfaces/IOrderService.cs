using RolleiShop.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int cartId);
    }
}
