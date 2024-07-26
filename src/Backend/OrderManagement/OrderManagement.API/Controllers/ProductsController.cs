using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("by-shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByShopId(int shopId)
        {
            var products = await _productService.GetProductsByShopIdAsync(shopId);
            return Ok(products);
        }
    }
}
