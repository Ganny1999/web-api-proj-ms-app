using order_api_ms.DomainModel.Models;
using order_api_ms.DomainModel.Models.Dtos;

namespace order_api_ms.Services.IExternalServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
