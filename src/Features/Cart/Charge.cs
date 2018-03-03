using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Entities;
using RolleiShop.Infrastructure;
using RolleiShop.Infrastructure.Interfaces;
using RolleiShop.Specifications;
using Stripe;

namespace RolleiShop.Features.Cart
{
    public class Charge
    {
        public class Query : IRequest
        {
            public string stripeEmail { get; set; }
            public string stripeToken { get; set; }
        }

        public class Handler : RequestHandler<Query>
        {
            protected override void HandleCore(Query message)
            {
                var customers = new StripeCustomerService();
                var charges = new StripeChargeService();
                var customer = customers.Create(new StripeCustomerCreateOptions {
                  Email = message.stripeEmail,
                  SourceToken = message.stripeToken
                });
                var charge = charges.Create(new StripeChargeCreateOptions {
                  Amount = 500,
                  Description = "Sample Charge",
                  Currency = "usd",
                  CustomerId = customer.Id
                });
            }
        }
    }
}
