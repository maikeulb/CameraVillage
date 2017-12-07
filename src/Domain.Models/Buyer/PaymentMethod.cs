namespace CameraVillage.Domain.Models
{
    public class PaymentMethod : Entity
    {
        public string Alias { get; set; }
        public string CardId { get; set; }
        public string Last4 { get; set; }

        private PaymentMethod () { }

        private PaymentMethod (string alias, string cardId, string last4) : this()
        {
            Alias = alias;
            CardId = cardId;
            Last4 = last4;
        }

        private static PaymentMethod Create (string alias, string cardId, string last4)
        {
            return new PaymentMethod (alias, cardId, last4);
        }
    }
}
