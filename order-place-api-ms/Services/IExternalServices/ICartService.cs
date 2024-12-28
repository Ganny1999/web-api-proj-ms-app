using order_place_api_ms.DomainModel.Models;

namespace order_place_api_ms.Services.IExternalServices
{
    public interface ICartService
    {
        Task<CartDetails> GetCartDetails(int CustomerID);
        Task<bool> RemoveCart(int CartItemsID);
    }
}
