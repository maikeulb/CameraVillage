using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using RolleiShop.Services.Interfaces;

namespace RolleiShop.Features.Cart
{
    public class Checkout
    {
        public class Command : IRequest
        {
            public Dictionary<string, int> Items { get; set; }
            public int Id { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;
            private readonly IOrderService _orderService;

            public Handler(ICartService cartService,
                    IOrderService orderService)
            {
                _cartService = cartService;
                _orderService = orderService;
            }

            protected override async Task HandleCore(Command message)
            {
                await _cartService.SetQuantities(message.Id, message.Items);
                await _orderService.CreateOrderAsync(message.Id);
                await _cartService.DeleteCartAsync(message.Id);
            }
        }
    }
}
