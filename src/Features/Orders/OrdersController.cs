using Stripe;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediatR;
using RolleiShop.Data.Context;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Specifications;

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
