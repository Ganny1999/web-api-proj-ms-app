using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using product_api_ms.Data;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
using product_api_ms.Filters;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace product_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly IProductService _productService;
        public readonly IMapper _mapper;
        public ProductController(AppDbContext context, IProductService productService, IMapper mapper)
        {
            _context = context;
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles ="ADMIN,USER")]
        // [Attribute("RequiredHttpsAttributeFilter")]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productService.GetAllProductAsync();
            return products;
        }

        [HttpPost]
        //[Authorize(Roles ="ADMIN")]
        public async Task<ActionResult<ProductDto>> AddProduct([FromBody] Product product)
        {

            var addProduct = await _productService.AddProductAsync(product);

            if (addProduct == null)
            {
                return NoContent();
            }

            return Ok(addProduct);
        }

        [HttpDelete("DeleteProduct/{prodID:int}")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> DeleteProduct(int prodID)
        {
            if(prodID >int.MinValue && prodID< int.MaxValue)
            {
                var isProductDeleted= await _productService.DeleteProductAsync(prodID);
                if(isProductDeleted) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateProduct")]
        //[Authorize(Roles = "ADMIN")]
        public ActionResult<Product> UpdateProduct([FromBody] Product product, int prodID)
        {
            if (product == null || prodID == null)
            {
                return BadRequest();
            }

            var isProductAvailable = _context.Products.FirstOrDefault(u => u.ProductID == prodID);
            if (isProductAvailable != null)
            {
                isProductAvailable.ProductName = product.ProductName;
                isProductAvailable.Price = product.Price;
                isProductAvailable.Discription = product.Discription;
                isProductAvailable.Originated = product.Originated;

                _context.Products.Update(isProductAvailable);
                _context.SaveChanges();

                var updatedProduct = _context.Products.FirstOrDefault(u => u.ProductID == prodID);

                return Ok(updatedProduct);
            }
            return Ok();
        }

        [HttpPatch]
        [Route("{prodID:int}/UpdatePartial")]
        //[Authorize(Roles = "ADMIN")]
        public ActionResult<ProductDto> UpdatePartial(int prodID, [FromBody] JsonPatchDocument<Product> productPatch)
        {
            try
            {
                if (productPatch == null || prodID <= 0)
                {
                    return BadRequest();
                }

                var isProductAvailable = _context.Products.FirstOrDefault(u => u.ProductID == prodID);
                if (isProductAvailable != null)
                {
                    var productToUpdate = new Product
                    {
                        ProductID = isProductAvailable.ProductID,
                        ProductName = isProductAvailable.ProductName,
                        Discription = isProductAvailable.Discription,
                        Originated = isProductAvailable.Originated,
                        Price = isProductAvailable.Price,
                        rating = isProductAvailable.rating
                    };

                    productPatch.ApplyTo(productToUpdate);

                    isProductAvailable.ProductName = productToUpdate.ProductName;
                    isProductAvailable.Discription = productToUpdate.Discription;
                    isProductAvailable.Originated = productToUpdate.Originated;
                    isProductAvailable.Price = productToUpdate.Price;
                    isProductAvailable.rating = productToUpdate.rating;

                    _context.Products.Update(isProductAvailable);
                    _context.SaveChanges();
                    return Created();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        // Stored Procedure data operation
        [HttpGet("CountOfProductBasedRating")]
        public object CountOfProductBasedRating(string Origion, int rating)
        {
            var products = _context.Products.FromSqlRaw("Exec GetProduct @rating = {0}, @Origion={1}", rating, Origion).ToList();
            //var products =  _context.Products.FromSqlRaw("Exec GetProduct @Rating = {0}", rating ]ToList();4

            var outputParameter = new SqlParameter
            {
                ParameterName = "@Value",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var res = _context.Database.ExecuteSqlRaw("Exec OutParameterDemo @Rating={0},@Value = @Value output", rating, outputParameter);
            return products.Count;
        }

        [HttpGet("SearchProductByName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProductByName(string ProductName)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductName))
                {
                    return BadRequest("Product name cannot be null or empty.");
                }
                var product = await _productService.SearchProductByName(ProductName);
                if (!product.IsNullOrEmpty())
                {
                    return Ok(product);
                }
                return NotFound($"Could not found product with name '{ProductName}'.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error occurred. Please try again!");
            }            
        }

        [HttpGet("AddFiltersProduct")]
        public async Task<ActionResult<ProductDto>> AddFiltersProduct(string ProductName,int minPrice, int maxPrice, int rating, string Originated)
        {
            //Need to implement.
            var filteredProducts = _context.Products.
               Where(u => u.ProductName.ToLower() == ProductName.ToLower() & (u.Price >= minPrice && u.Price <= maxPrice) & u.rating == rating & u.Originated.ToLower() == Originated.ToLower());

            if (filteredProducts == null)
            {
                return NotFound("Result not fund.");
            }
            return Ok(filteredProducts);
        }

        [HttpGet("UpdateRating")]
        public async Task<bool> UpdateRating()
        {
            var result = await _productService.UpdateProductRating();
            return result;
        }

        [HttpGet("GetProductByID/{ProductID:int}")]
        public async Task<ActionResult<ProductDto>> GetProductByID(int ProductID)
        {
            var product = await _productService.GetProductByIdAsync(ProductID);
            if (product == null)
            {
                return NotFound("Either invalid product ID or Product with ID not found.");
            }
            return product;
        }
    }
}