using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace RolleiShop.Features.Orders
{
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(Index.Query query)
        {
            query.Name = User.Identity.Name;
            var model = await _mediator.Send(query);

            return View(model);
        }

        public async Task<IActionResult> Details(Details.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }
    }
}
