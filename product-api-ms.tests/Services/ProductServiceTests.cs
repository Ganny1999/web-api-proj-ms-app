using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using product_api_ms.Data;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
using product_api_ms.Services;
using product_api_ms.Services.IExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace product_api_ms.tests.Services
{
    public class ProductServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<IRatingService> _ratingService;
        private readonly Mock<IMapper> _mapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            //Mock Database
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _dbContext = new AppDbContext(options);

            

            //// Mock dependencies
            _mapper = new Mock<IMapper>();
            _ratingService = new Mock<IRatingService>();

            // Inserting the dependency
            _productService = new ProductService(_dbContext, _mapper.Object, _ratingService.Object);
        }
        
        [Fact]
        public async Task GetAllProducts()
        {
            // Arrange
            var product = AddProductsToList();
            var productDto = AddProductsDtoToList();

            await _dbContext.Products.AddRangeAsync(product);
            await _dbContext.SaveChangesAsync();

            _mapper.Setup(m=>m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>())).Returns(productDto);

            // Act
            var result = await _productService.GetAllProductAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2,result.Count());
            //Assert.Equal(productID,result.ProductID);
        }
        [Fact]
        public async Task GetProductByID()
        {
            //Arrange
            var product = AddProductsToList();
            var productDto = AddProductsDtoToList();

            await _dbContext.AddRangeAsync(product);
            await _dbContext.SaveChangesAsync();

            _mapper.Setup(m=>m.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto.FirstOrDefault(u=>u.ProductID==1));

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            //Assert
            Assert.Equal(1,result.ProductID);
        }
        private List<Product> AddProductsToList()
        {
            var product = new List<Product>()
            {
                new Product{
                    ProductID = 1,
                    ProductName = $"Product 1",
                    Price = 10 * 1,
                    Discription = "Discription for product 1",
                    Originated = $"Originated 1",
                    rating = 1, },
                new Product{
                    ProductID = 2,
                    ProductName = $"Product 1=2",
                    Price = 10 * 2,
                    Discription = $"Discription for product 2",
                    Originated = $"Originated 2",
                    rating = 2, },
            };
            return product; 
        }
        private List<ProductDto> AddProductsDtoToList()
        {
            var product = new List<ProductDto>()
            {
                new ProductDto{
                    ProductID = 1,
                    ProductName = $"Product 1",
                    Price = 10 * 1,
                    Discription = "Discription for product 1",
                    Originated = $"Originated 1",
                    rating = 1, },
                new ProductDto{
                    ProductID = 2,
                    ProductName = $"Product 1=2",
                    Price = 10 * 2,
                    Discription = $"Discription for product 2",
                    Originated = $"Originated 2",
                    rating = 2, },
            };
            return product;
        }
    }
}
