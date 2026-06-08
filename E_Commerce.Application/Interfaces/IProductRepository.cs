using E_Commerce.Domain.Entities;
namespace E_Commerce.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds);
    }
}
