using product_api_ms.DomainModel.Models;

namespace product_api_ms.DomainModel.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int ProductID);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int ProductID);
    }
}
