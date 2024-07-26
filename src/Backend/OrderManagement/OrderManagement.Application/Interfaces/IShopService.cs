using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IShopService
    {
        Task<IEnumerable<ShopDto>> GetAllShopsAsync();
    }
}
