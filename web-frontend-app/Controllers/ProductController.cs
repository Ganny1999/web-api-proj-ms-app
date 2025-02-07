using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_frontend_app.Models.DTOs;
using web_frontend_app.Services.IServices;

namespace web_frontend_app.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
                _productService= productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            IEnumerable<ProductDto> products = await _productService.GetProductAsync();
            return View(products.ToList());
        }
    }
}
