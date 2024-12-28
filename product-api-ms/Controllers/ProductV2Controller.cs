using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using product_api_ms.DomainModel.DTOs;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.DomainModel.Models;
using product_api_ms.Filters;

// Coontroller is created to maintain the versioning

namespace product_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /*
    Versioning : its working 
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    */

    //[ServiceFilter(typeof(CustomAuthenticationFilter))]
    public class ProductV2Controller : ControllerBase
    {
        public readonly IProductService _productService;
        public readonly IMapper _mapper;
        public ProductV2Controller(IProductService productService, IMapper mapper)
        {
                _productService= productService;
            _mapper= mapper;
        }
        [HttpGet("GetProducts")]
        //[ServiceFilter(typeof(CustomResourceFilter))]
        //[ServiceFilter(typeof(CustomExceptionFilter))]
        //[ServiceFilter(typeof(CustomActionFilter))]
        [ServiceFilter(typeof(CustomResultFilter))]
        public Task<IEnumerable<ProductDto>> GetProducts()
        {
            // For Exception filter example
            //throw new Exception("Something went wrong, Kindly investigate!!!");

            var products = _productService.GetAllProductAsync();
            return products;
        }
        [HttpPost("AddProduct")]
        [ServiceFilter(typeof(CustomActionFilter))]
        public Product AddProduct([FromBody] Product product)
        {
            return new Product { };
        }
    }
}
