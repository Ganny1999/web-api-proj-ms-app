using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using order_api_ms.DomainModel.IServices;
using order_api_ms.DomainModel.Models;

namespace order_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        [Route("GetCart")]
        public async Task<CartDetails> GetCart([FromBody]int CustomerID)
        {
            try
            {
                var cart = await _cartService.GetCart(CustomerID);
                if(cart != null) 
                {
                    return cart;
                }

                return new CartDetails { };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("UpsertCar")]
        public async Task<Cart> UpsertCart([FromBody] CartDetails cartDetails)
        {
            if (cartDetails == null)
            {
                return new Cart();
            }
            var UpsertedCart = await _cartService.UpsertCartAsync(cartDetails);
            return UpsertedCart;
        }
        [HttpDelete]
        [Route("RemoveCart")]
        public async Task<bool> RemoveCart([FromBody] int CartItemsID)
        {
            try
            {
                var IsCartItemDeleted = await _cartService.RemoveCartAsync(CartItemsID);
                if (IsCartItemDeleted)
                {
                    return true;
                }
                else return false;
            }
            catch(Exception  ex)
            {
                throw ex;
            }            
        }
    }
}
