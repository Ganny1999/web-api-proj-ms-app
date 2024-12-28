using order_place_api_ms.DomainModel.Dtos;

namespace order_place_api_ms.Services.IExternalServices
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByID(int productID);
    }
}
