using System.Collections.Generic;
using System.Threading.Tasks;
using RolleiShop.Entities;

namespace RolleiShop.Services.Interfaces
{
    public interface ICartService
    {
        Task<int> GetCartItemCountAsync(string userName);
        Task AddItemToCart(int cartId, int catalogItemId, decimal price, int quantity);
        Task RemoveItemFromCart(int cartId, int catalogItemId);
        Task SetQuantities(int cartId, Dictionary<string, int> quantities);
        Task DeleteCartAsync(int cartId);
        Task TransferCartAsync(string anonymousId, string userName);
        Task<IEnumerable<string>> GetUsers ();
        Task<Cart> GetCartAsync(string userName);
        Task<IEnumerable<Cart>> GetCartsWithMatchingProductId(int productId);
        Task UpdateAsync(Cart cart);
    }
}
