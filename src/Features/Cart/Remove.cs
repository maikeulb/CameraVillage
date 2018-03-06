using System.Threading.Tasks;
using MediatR;
using RolleiShop.Services.Interfaces;

namespace RolleiShop.Features.Cart
{
    public class Remove
    {
        public class Command : IRequest
        {
            public int CatalogItemId { get; set; }
            public int Id { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;

            public Handler(ICartService cartService)
            {
                _cartService = cartService;
            }

            protected override async Task HandleCore(Command message)
            {
                await _cartService.RemoveItemFromCart(message.Id, message.CatalogItemId);
            }
        }
    }
}
