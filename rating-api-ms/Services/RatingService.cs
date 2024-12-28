using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rating_api_ms.Data;
using rating_api_ms.DomainModel.Dtos;
using rating_api_ms.DomainModel.IServices;
using rating_api_ms.DomainModel.Models;
using System.Runtime.InteropServices;

namespace rating_api_ms.Services
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RatingService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RatingDto> AddRating(RatingDto ratingDto)
        {
            if(ratingDto==null)
            {
                throw new NullReferenceException(nameof(ratingDto));
            }
            try
            {
                var isRatingExist = await _context.Ratings.FirstOrDefaultAsync(u => u.UserID == ratingDto.UserID && u.ProductID == ratingDto.ProductID);

                if (isRatingExist == null)
                {
                    var rating = _mapper.Map<Rating>(ratingDto);
                    await _context.Ratings.AddAsync(rating);
                    await _context.SaveChangesAsync();

                    var IsRatingAdded = _mapper.Map<RatingDto>(await _context.Ratings.FirstOrDefaultAsync(u => u.UserID == ratingDto.UserID && u.ProductID == ratingDto.ProductID));
                    return IsRatingAdded;
                }
                return _mapper.Map<RatingDto>(isRatingExist);
            }
            catch { throw; }
        }

        public async Task<IEnumerable<RatingDto>> GetAllRating()
        {
            var ratingList = await _context.Ratings.ToListAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratingList);
        }

        public async Task<RatingDto> UpdateRating(UpdateRatingDto updateRatingDto)
        {
            try
            {
                var isRatingExist = await _context.Ratings.FirstOrDefaultAsync(u=>u.UserID== updateRatingDto.UserID &&  u.ProductID== updateRatingDto.ProductID);
                if (isRatingExist != null)
                {
                    isRatingExist.RatingValue = updateRatingDto.RatingValue;
                    await _context.SaveChangesAsync();

                    var updatedrating = _mapper.Map<RatingDto>(isRatingExist);
                    return updatedrating;
                }
                return null;
            }
            catch(Exception ex){ throw; }
        }
        public async Task<decimal> AverageRatingOfProduct(int ProductID)
        {
            var res = await _context.AverageProductRatings.FirstOrDefaultAsync(u=>u.ProductID==ProductID);
            if(res != null)
            {
                return res.AverageRating;
            }
            return 0;
        }
    }
}