using order_api_ms.DomainModel.Models;

namespace order_api_ms.DomainModel.IServices
{
    public interface IOrderService
    {
        Task<Order> PlaceOder(Order order);
    }
}
