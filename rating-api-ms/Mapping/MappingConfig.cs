using AutoMapper;
using rating_api_ms.DomainModel.Dtos;
using rating_api_ms.DomainModel.Models;

namespace rating_api_ms.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Rating,RatingDto>().ReverseMap();
        }
    }
}
