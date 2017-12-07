using CameraVillage.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CameraVillage.Domain.Models.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrder(int basketId, Address shippingAddress);
   }
}
