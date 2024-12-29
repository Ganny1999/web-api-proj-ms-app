using Newtonsoft.Json;
using order_place_api_ms.DomainModel.Dtos;
using order_place_api_ms.DomainModel.Models;
using order_place_api_ms.Services.IExternalServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace order_place_api_ms.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
        public async Task<CartDetails> GetCartDetails(int CustomerID)
        {
            var requestPayload = new CartRequestDto()
            {
                CustomerID= CustomerID
            };
            var JsonPayload = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(JsonPayload, Encoding.UTF8, "application/json");
            
            var client = _httpClientFactory.CreateClient("Cart");
            var response = await client.PostAsync($"/api/cart/GetCart/{CustomerID}", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var cartDetails = JsonConvert.DeserializeObject<CartDetails>(responseContent);
            return cartDetails;
        }

        public async Task<Cart> RemoveCart(int cartID)
        {
            var requestPayload = new CartRemoveRequestDto()
            {
                CartID = cartID
            };

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonPayload,Encoding.UTF8,"application/json");

            var client = _httpClientFactory.CreateClient("Cart");
            var response = await client.PostAsync($"/api/Cart/RemoveCart/{cartID}",content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var isSuccess = JsonConvert.DeserializeObject<Cart>(responseContent);

            return isSuccess;
        }
    }
}
