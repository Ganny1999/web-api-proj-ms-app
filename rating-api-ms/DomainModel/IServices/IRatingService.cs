using Microsoft.AspNetCore.Mvc;
using rating_api_ms.DomainModel.Dtos;
using rating_api_ms.DomainModel.Models;

namespace rating_api_ms.DomainModel.IServices
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingDto>> GetAllRating();
        Task<RatingDto> AddRating(RatingDto rating);
        Task<RatingDto> UpdateRating(UpdateRatingDto updateRatingDto);
        Task<decimal> AverageRatingOfProduct(int ProductID);

    }
}
