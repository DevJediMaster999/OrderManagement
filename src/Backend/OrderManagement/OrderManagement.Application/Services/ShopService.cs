using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Application.Services
{
    public class ShopService : IShopService
    {
        private readonly AppDbContext _context;

        public ShopService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShopDto>> GetAllShopsAsync()
        {
            var shops = await _context.Shops
                .Where(s => s.isEnabled)
                .Select(s => new ShopDto
                {
                    IdShop = s.Id,
                    ShopName = s.Name
                })
                .ToListAsync();

            return shops;
        }
    }
}
