using AutoMapper;
using order_api_ms.DomainModel.Models.Dtos;
using order_api_ms.DomainModel.Models;

namespace order_api_ms.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig() 
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
