using Microsoft.EntityFrameworkCore;
using RolleiShop.Models.Interfaces;
using RolleiShop.Models.Order;
using RolleiShop.Models.Entities;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.Data.Repositories
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(CatalogContext dbContext) : base(dbContext) {}

        public Order GetByIdWithItems(int id)
        {
            return _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include("OrderItems.ItemOrdered")
                .FirstOrDefault();
        }

        public Task<Order> GetByIdWithItemsAsync(int id)
        {
            return _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include("OrderItems.ItemOrdered")
                .FirstOrDefaultAsync();
        }
    }
}
