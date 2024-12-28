
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using order_api_ms.DomainModel.Models;
using order_api_ms.DomainModel.Models.Dtos;
using order_api_ms.Services.IExternalServices;
using System.Text.Json.Serialization;


namespace order_api_ms.Services
{
    public class ProductService : IProductService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory  httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            
            var client = _httpClientFactory.CreateClient("Product");

            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(apiContent);
            if(!resp.IsNullOrEmpty())
            {
                return resp;
            }
            return new List<ProductDto>();
        }
    }
}
