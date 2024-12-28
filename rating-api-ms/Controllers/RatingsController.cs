using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rating_api_ms.DomainModel.Dtos;
using rating_api_ms.DomainModel.IServices;

namespace rating_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingsController(IRatingService ratingService)
        {
                _ratingService = ratingService;
        }

        [HttpGet("GetAllRating")]
        public Task<IEnumerable<RatingDto>> GetAllRating() 
        {
            var AllRatingList = _ratingService.GetAllRating();
            return AllRatingList;
        }
        [HttpPost("AddRatingToProduct")]
        public async Task<ActionResult<RatingDto>> AddRatingToProduct([FromBody] RatingDto rating)
        {
            var isAdded = await _ratingService.AddRating(rating);
            if(isAdded != null)
            {
                return Ok(isAdded);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<ActionResult<RatingDto>> UpdateRating([FromBody] UpdateRatingDto updateRating)
        {
           var IsRatingUpdated = await _ratingService.UpdateRating(updateRating);
            if(IsRatingUpdated !=null)
            {
                return Ok(IsRatingUpdated);
            }
            return NotFound("Rating not found for the specified user and product.");
        }

        // it will fetch the average rating of each product and update the product records.
        [HttpGet("AverageRatingProduct/{ProductID}")]
        public Task<decimal> AverageRatingProduct(int ProductID)
        {
           var result =  _ratingService.AverageRatingOfProduct(ProductID);
            return result;
        }
    }
}
