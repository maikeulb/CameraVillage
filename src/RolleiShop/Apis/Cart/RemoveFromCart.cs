using System.Threading.Tasks;
using MediatR;
using RolleiShop.Data.Context;
using RolleiShop.Services.Interfaces;

namespace RolleiShop.Apis.Cart
{
    public class RemoveFromCart
    {
        public class Command : IRequest
        {
            public int ProductId { get; set; }
            public int Id { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;
            private readonly ApplicationDbContext _context;

            public Handler (ICartService cartService,
                ApplicationDbContext context)
            {
                _cartService = cartService;
                _context = context;
            }

            protected override async Task HandleCore (Command message)
            {
                var catalogItem = await _context.CatalogItems.FindAsync (message.ProductId);
                await _cartService.RemoveItemFromCart (message.Id, catalogItem.Id);
            }
        }
    }
}