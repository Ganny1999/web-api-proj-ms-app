using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Models;

namespace product_api_ms.DomainModel.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductAsync();
        Task<ProductDto> GetProductByIdAsync(int ProductID);
        Task<ProductDto> AddProductAsync(Product product);
        Task<ProductDto> UpdateProductAsync(Product product);
        Task<IEnumerable<ProductDto>> SearchProductByName(string ProductKeyword);
        Task<bool> DeleteProductAsync(int ProductID);
        Task<bool> UpdateProductRating();
    }
}
