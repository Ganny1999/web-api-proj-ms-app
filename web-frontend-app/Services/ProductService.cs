using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using web_frontend_app.Models.DTOs;
using web_frontend_app.Services.IServices;

namespace web_frontend_app.Services
{
    public class ProductService : IProductService
    {
        public IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("Product");
            var response = await httpClient.DeleteAsync("/api/product/DeleteProduct/" + id);            
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ProductDto>> GetProductAsync()
        {
             var httpClient = _httpClientFactory.CreateClient("Product");

            var response = await httpClient.GetAsync("/api/product");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(content);
            return resp;
        }
    }
}
