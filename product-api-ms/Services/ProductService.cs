using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using product_api_ms.Data;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
using System.Net.WebSockets;

namespace product_api_ms.Services
{
    public class ProductService : IProductService
    {
        public readonly AppDbContext _dbContext;
        public readonly IMapper _mapper;
        public ProductService(AppDbContext dbContext,IMapper mapper)
        {
                _dbContext=dbContext;
            _mapper=mapper;
        }
        public async Task<ProductDto> AddProductAsync(Product product)
        {
            if(product==null)
            {
                throw new ArgumentNullException(nameof(product),"Product cannot be null");
            }

            var isEstist = await _dbContext.Products.FirstOrDefaultAsync(u=>u.ProductName==product.ProductName);
            if(isEstist==null)
            {
                var addedProdut = await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                var adedProductDto = _mapper.Map<ProductDto>(addedProdut);

                return adedProductDto; 
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

        public Task<ProductDto> GetProductByIdAsync(int ProductID)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
