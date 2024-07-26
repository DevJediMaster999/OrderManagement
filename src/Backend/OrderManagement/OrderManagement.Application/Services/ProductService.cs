using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByShopIdAsync(int shopId)
        {
            var products = await _context.Products
                .Where(p => p.ShopProducts.Any(sp => sp.ShopId == shopId))
                .Select(p => new ProductDto
                {
                    IdProduct = p.Id,
                    ProductName = p.Name,
                    Price = p.Price
                })
                .ToListAsync();

            return products;
        }
    }
}
