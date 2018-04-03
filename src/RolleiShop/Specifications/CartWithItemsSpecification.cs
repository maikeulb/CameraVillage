using RolleiShop.Entities;

namespace RolleiShop.Specifications
{
    public class CartWithItemsSpecification : BaseSpecification<Cart>
    {
        public CartWithItemsSpecification(int cartId)
            :base(b => b.Id == cartId)
        {
            AddInclude(b => b.Items);
        }
        public CartWithItemsSpecification(string buyerId)
            :base(b => b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }
}
