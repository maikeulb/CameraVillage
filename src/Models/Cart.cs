using System.Collections.Generic;
using System.Linq;

namespace RolleiShop.Models.Entities
{
    public class Cart : Entity
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public string BuyerId { get; set; }
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add(new CartItem()
                {
                    CatalogItemId = catalogItemId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                });
                return;
            }
            var existingItem = Items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
            existingItem.Quantity += quantity;
        }

        public void RemoveItem(int catalogItemId)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                /* _items = _items.Where(item => item.Id != id).ToList(); */
                var item = _items.SingleOrDefault(x=>x.Id == catalogItemId);
                if (item != null)
                  _items.Remove(item);
                return;
            }
        }

    }
}
