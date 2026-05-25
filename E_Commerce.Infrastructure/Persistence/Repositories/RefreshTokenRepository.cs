

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
        public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
        {
            return await _context.RefreshTokens.Include(rt => rt.Customer).SingleOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
