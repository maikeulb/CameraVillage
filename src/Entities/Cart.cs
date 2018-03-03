using System.Collections.Generic;
using System.Linq;

namespace RolleiShop.Entities
{
    public class Cart : Entity
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public string BuyerId { get; private set; }
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        private Cart () {}

        private Cart (string buyerId)
        {
            BuyerId = buyerId;
        }

        public static Cart Create (string buyerId) 
        {
            return new Cart (buyerId);
        }

        public void TransferCart(string buyerId)
        {
            BuyerId = buyerId;
        }

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add( CartItem.Create(
                    unitPrice,
                    quantity,
                    catalogItemId
                ));
                return;
            }
            var existingItem = Items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
            existingItem.IncrementQuantity();
        }

        public void RemoveItem(int catalogItemId)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                var item = _items.SingleOrDefault(x=>x.Id == catalogItemId);
                if (item != null)
                  _items.Remove(item);
                return;
            }
        }
    }
}
