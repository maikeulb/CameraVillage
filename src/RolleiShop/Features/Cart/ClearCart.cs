using System.Threading.Tasks;
using MediatR;
using RolleiShop.Services.Interfaces;

namespace RolleiShop.Features.Cart
{
    public class ClearCart
    {
        public class Command : IRequest
        {
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
                await _cartService.DeleteCartAsync(message.Id);
            }
        }
    }
}
