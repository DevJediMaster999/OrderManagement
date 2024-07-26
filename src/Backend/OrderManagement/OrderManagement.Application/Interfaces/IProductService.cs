using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsByShopIdAsync(int shopId);
    }
}
