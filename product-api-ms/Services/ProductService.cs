using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using product_api_ms.Data;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
using product_api_ms.Services.IExternalServices;
using System.Net.WebSockets;

namespace product_api_ms.Services
{
    public class ProductService : IProductService
    {
        public readonly AppDbContext _dbContext;
        private readonly IRatingService _routingService;
        public readonly IMapper _mapper;
        public ProductService(AppDbContext dbContext,IMapper mapper, IRatingService routingService)
        {
                _dbContext=dbContext;
                _mapper=mapper;
                _routingService=routingService;
        }
        public async Task<ProductDto> AddProductAsync (Product product)
        {
            if(product==null)
            {
                throw new ArgumentNullException(nameof(product),"Product cannot be null");
            }

            var isEstist = await _dbContext.Products.FirstOrDefaultAsync(u=>u.ProductName==product.ProductName);
            if(isEstist==null)
            {
                var isSuccess = await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                var IsAdded = await _dbContext.Products.FirstOrDefaultAsync(u => u.ProductName == product.ProductName);
                var addedProductDto = _mapper.Map<ProductDto>(IsAdded);

                return addedProductDto; 
            }

            return new ProductDto { };
        }

        public Task DeleteProductAsync(int ProductID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            var AllProductList= await _dbContext.Products.ToListAsync();
            var AppProductListDto =  _mapper.Map<IEnumerable<ProductDto>>(AllProductList);   
            return AppProductListDto;   
        }

        public async Task<ProductDto> GetProductByIdAsync(int ProductID)
        {
            if(ProductID<=0)
            {
                return null;
            }
            var isProductExists = await _dbContext.Products.FirstOrDefaultAsync(u=>u.ProductID == ProductID);
            if(isProductExists!=null) 
            {
                var productDto = _mapper.Map<ProductDto>(isProductExists);
                return productDto;
            }
            return null;
        }

        public Task<ProductDto> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateProductRating()
        {
            try
            {
                var ProductList = await _dbContext.Products.ToListAsync();
                foreach (var product in ProductList)
                {
                    product.rating = await _routingService.GetAverageRating(product.ProductID);

                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error updating product ratings: {ex.Message}");

                return false;
            }
            
        }
    }
}
