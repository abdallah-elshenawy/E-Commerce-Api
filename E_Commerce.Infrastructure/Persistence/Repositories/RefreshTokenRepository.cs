

using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(rt => rt.Customer).SingleOrDefaultAsync(rt => rt.Token == token);
        }
        public async Task RevokeAllTokensForCustomerAsync(int customerId)
        {
            await _context.RefreshTokens.Where(rt => rt.CustomerId == customerId && !rt.IsRevoked)
                            .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));
        }

        public async Task<int> DeleteExpiredTokensAsync()
        {
            return await _context.RefreshTokens.Where(rt => rt.IsRevoked || rt.IsUsed || rt.ExpiresAt < DateTime.UtcNow)
                .ExecuteDeleteAsync();
        }
    }
}
