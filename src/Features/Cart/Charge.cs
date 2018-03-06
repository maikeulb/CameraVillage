using MediatR;
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
