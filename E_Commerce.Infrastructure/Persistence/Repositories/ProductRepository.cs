using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products.AsNoTracking().Where(p => p.StockQuantity < threshold).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }
    }
}
