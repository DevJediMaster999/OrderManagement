using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.API.Controllers
{
    [Route("api/shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetAllShops()
        {
            var shops = await _shopService.GetAllShopsAsync();
            return Ok(shops);
        }
    }
}
