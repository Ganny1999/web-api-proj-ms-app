using order_place_api_ms.DomainModel.Dtos;
using order_place_api_ms.DomainModel.Models;

namespace order_place_api_ms.DomainModel.IServices
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(int CustomerID);
        Task<Order> GetOrderByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}
