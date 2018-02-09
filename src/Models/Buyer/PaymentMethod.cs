using Rolleishop.Models.Entities;

namespace RolleiShop.Models.Entities.Buyer
{
    public class PaymentMethod : Entity
    {
        public string Alias { get; set; }
        public string CardId { get; set; }
        public string Last4 { get; set; }
    }
}
