using RolleiShop.Entities;
using System.Collections.Generic;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RolleiShop.Notifications
{
    public class ProductPriceChangedNotification : INotification
    {        
        public DateTime CreationDate { get; }

        public int ProductId { get; private set; }

        public decimal NewPrice { get; private set; }

        public decimal OldPrice { get; private set; }

        public ProductPriceChangedNotification(int productId, decimal newPrice, decimal oldPrice)
        {
            CreationDate = DateTime.UtcNow;
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }

    public class ProductPriceChangedNotificationHandler : INotificationHandler<ProductPriceChangedNotification>
    {
        private readonly ICartService _cartService;

        public ProductPriceChangedNotificationHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Handle(ProductPriceChangedNotification notification, CancellationToken cancellationToken)
        {
            var userIds = await _cartService.GetUsers();
            var iterlist = userIds.ToList(); // TODO CLEANUP
            foreach (var id in iterlist)
            {
                var cart = await _cartService.GetCartAsync(id);

                await UpdatePriceInCartItems(notification.ProductId, notification.NewPrice, notification.OldPrice, cart);                      
            }
        }

        private async Task UpdatePriceInCartItems(int productId, decimal newPrice, decimal oldPrice, Cart cart)
        {
            IEnumerable<Cart> cartsToUpdate = await _cartService.GetCartsWithMatchingProductId(productId); // TODO make one command (linq -> select)

            IEnumerable<CartItem> itemsToUpdate = Enumerable.Empty<CartItem>();
                
            foreach (var cartp in cartsToUpdate)
            {
                itemsToUpdate = cartp.Items; 
            } 

            if (itemsToUpdate != null)
            {
                foreach (var item in itemsToUpdate)
                {
                        item.UnitPrice = newPrice;
                }
                await _cartService.UpdateAsync(cart);
            }         
        }

        /* private async Task UpdatePriceInCatalog(int productId, decimal newPrice, decimal oldPrice, Cart cart) */
        /* { */
        /*     IEnumerable<Cart> cartsToUpdate = await _cartService.GetCartsWithMatchingProductId(productId); // TODO make one command (linq -> select) */

        /*     IEnumerable<CartItem> itemsToUpdate = Enumerable.Empty<CartItem>(); */
                
        /*     foreach (var cartp in cartsToUpdate) */
        /*     { */
        /*         itemsToUpdate = cartp.Items; */ 
        /*     } */ 

        /*     if (itemsToUpdate != null) */
        /*     { */
        /*         foreach (var item in itemsToUpdate) */
        /*         { */
        /*                 item.UnitPrice = newPrice; */
        /*         } */
        /*         await _cartService.UpdateAsync(cart); */
        /*     } */         
        /* } */
    }
}
