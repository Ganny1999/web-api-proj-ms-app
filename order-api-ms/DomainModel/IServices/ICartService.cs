using order_api_ms.DomainModel.Models;

namespace order_api_ms.DomainModel.IServices
{
    public interface ICartService
    {
        Task<Cart> UpsertCartAsync(CartDetails cartDetails);
        Task<bool> RemoveCartAsync(int CartDetailsID);
        Task<CartDetails> GetCart(int CustomerID);

    }
}
