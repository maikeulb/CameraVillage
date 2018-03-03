using RolleiShop.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Rolleishop.Notifications
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

    /* public class ProductPriceChangedNotificationHandler : IAsyncNotificationHandler<ProdutPriceChangedNotification> */
    /* { */
    /*     private readonly ApplicationDbContext _context; */

    /*     public ProductPriceChangedIntegrationEventHandler(IdentityDbContext context) */
    /*     { */
    /*         _context = context */
    /*     } */

    /*     public async Task Handle(ProductPriceChangedNotification notification) */
    /*     { */
    /*         var userIds = _context.ApplicationUsers.GetUsers(); */
            
    /*         foreach (var id in userIds) */
    /*         { */
    /*             var cart = await GetCartAsync(id) */

    /*             await UpdatePriceInCartItems(notification.ProductId, notification.NewPrice, notification.OldPrice, cart); */                      
    /*         } */
    /*     } */

    /*     private async Task UpdatePriceInCartItems(int productId, decimal newPrice, decimal oldPrice, CustomerCart cart) */
    /*     { */
    /*         string match = productId.ToString(); */
    /*         var itemsToUpdate = cart?.Items?.Where(x => x.ProductId == match).ToList(); */

    /*         if (itemsToUpdate != null) */
    /*         { */
    /*             foreach (var item in itemsToUpdate) */
    /*             { */
    /*                 if(item.UnitPrice == oldPrice) */
    /*                 { */ 
    /*                     var originalPrice = item.UnitPrice; */
    /*                     item.UnitPrice = newPrice; */
    /*                     item.OldUnitPrice = originalPrice; */
    /*                 } */
    /*             } */
    /*             await _repository.UpdateCartAsync(cart); */
    /*         } */         
    /*     } */

    /*     private async Task GetCartAsync(int productId, decimal newPrice, decimal oldPrice, CustomerCart cart) */
    /*     { */
    /*         string match = productId.ToString(); */
    /*         var itemsToUpdate = cart?.Items?.Where(x => x.ProductId == match).ToList(); */

    /*         if (itemsToUpdate != null) */
    /*         { */
    /*             foreach (var item in itemsToUpdate) */
    /*             { */
    /*                 if(item.UnitPrice == oldPrice) */
    /*                 { */ 
    /*                     var originalPrice = item.UnitPrice; */
    /*                     item.UnitPrice = newPrice; */
    /*                     item.OldUnitPrice = originalPrice; */
    /*                 } */
    /*             } */
    /*             await _repository.UpdateCartAsync(cart); */
    /*         } */         
    /*     } */

    /*     private async Task UpdateCartAsync(int productId, decimal newPrice, decimal oldPrice, CustomerCart cart) */
    /*     { */
    /*         var created = await _context. */
    /*     } */
    /* } */
}
