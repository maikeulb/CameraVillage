using RolleiShop.Models.Entities.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int cartId, Address shippingAddress);
    }
}
