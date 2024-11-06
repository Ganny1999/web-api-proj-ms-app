using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using product_api_ms.Data;
using product_api_ms.Models;

namespace product_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context=context;
        }
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            var products = _context.Products;
            return products;
        }
        [HttpPost]
        public ActionResult<Product> AddProduct([FromBody] Product product)
        {
            if( product == null)
            {
                return BadRequest();
            }
            var res = _context.Products.Add(product);

            var isAdded = _context.Products.FirstOrDefault(u=>u.ProductName==product.ProductName);
            _context.SaveChanges();
            return isAdded;
        }
    }
}
