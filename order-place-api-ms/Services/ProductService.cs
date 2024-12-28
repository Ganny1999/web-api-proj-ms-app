using Newtonsoft.Json;
using order_place_api_ms.DomainModel.Dtos;
using order_place_api_ms.Services.IExternalServices;

namespace order_place_api_ms.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory=httpClientFactory;
        }
        public async Task<ProductDto> GetProductByID(int productID)
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/GetProductByID/{productID}");
            var apiContent = response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var product = JsonConvert.DeserializeObject<ProductDto>(apiContent.Result);

            if(product != null)
            {
                return product;
            }
            return null;
        }
    }
}
