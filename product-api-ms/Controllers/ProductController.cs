using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using product_api_ms.Data;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
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
        public ProductController(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        [HttpGet]
        public Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = _productService.GetAllProductAsync();
            return products;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {

            var addProduct = await _productService.AddProductAsync(product);

            if(addProduct==null)
            {
                return NoContent();
            }

            return Ok(addProduct);
            //=============================================================
            /*
            var isExist = _context.Products.FirstOrDefault(u => u.ProductName == product.ProductName);
            if (product == null)
            {
                return BadRequest();
            }
            if (isExist == null)
            {
                var res = _context.Products.Add(product);

                var isAdded = _context.Products.FirstOrDefault(u => u.ProductName == product.ProductName);
                _context.SaveChanges();
                return isAdded;
            }
            return Ok(product);

            */
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public ActionResult<Product> DeleteProduct(int prodID)
        {
            var isProductAvailable = _context.Products.FirstOrDefault(u=>u.ProductID==prodID);
            if(isProductAvailable!=null)
            { 
                var product = _context.Products.Remove(isProductAvailable);
                _context.SaveChanges();
                return Ok(isProductAvailable);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("UpdateProduct")]
        public ActionResult<Product> UpdateProduct([FromBody] Product product, int prodID)
        {
            if(product == null || prodID== null)
            {
                return BadRequest();
            }

            var isProductAvailable = _context.Products.FirstOrDefault(u => u.ProductID == prodID);
            if(isProductAvailable != null)
            {
                isProductAvailable.ProductName=product.ProductName;
                isProductAvailable.Price=product.Price;
                isProductAvailable.Discription=product.Discription;
                isProductAvailable.Originated=product.Originated;

                _context.Products.Update(isProductAvailable);
                _context.SaveChanges();

                var updatedProduct = _context.Products.FirstOrDefault(u=>u.ProductID == prodID);

                return Ok(updatedProduct);
            }
            return Ok();
        }
        [HttpPatch]
        [Route("{prodID:int}/UpdatePartial")]
        public ActionResult<Product> UpdatePartial(int prodID, [FromBody] JsonPatchDocument<Product> productPatch)
        {
            try
            {
                if (productPatch == null || prodID<=0)
                {
                    return BadRequest();
                }

                var isProductAvailable = _context.Products.FirstOrDefault(u => u.ProductID == prodID);
                if(isProductAvailable != null)
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

                    isProductAvailable.ProductName=productToUpdate.ProductName;
                    isProductAvailable.Discription = productToUpdate.Discription;
                    isProductAvailable.Originated = productToUpdate.Originated;
                    isProductAvailable.Price=productToUpdate.Price;
                    isProductAvailable.rating = productToUpdate.rating;

                    _context.Products.Update(isProductAvailable);
                    _context.SaveChanges();
                    return Created();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Ok();
        }
    }
}
