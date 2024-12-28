using web_frontend_app.Models.DTOs;

namespace web_frontend_app.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductAsync();
    }
}
