using MediatR;
using Stripe;

namespace RolleiShop.Features.Cart
{
    public class Charge
    {
        public class Query : IRequest
        {
            public string StripeEmail { get; set; }
            public string StripeToken { get; set; }
            public decimal StripeTotal { get; set; }
        }

        public class Handler : RequestHandler<Query>
        {
            protected override void HandleCore(Query message)
            {
                var customers = new StripeCustomerService();
                var charges = new StripeChargeService();
                var customer = customers.Create(new StripeCustomerCreateOptions {
                  Email = message.StripeEmail,
                  SourceToken = message.StripeToken
                });
                var charge = charges.Create(new StripeChargeCreateOptions {
                  Amount = (int) message.StripeTotal,
                  Description = "Sample Charge",
                  Currency = "usd",
                  CustomerId = customer.Id
                });
            }
        }
    }
}
