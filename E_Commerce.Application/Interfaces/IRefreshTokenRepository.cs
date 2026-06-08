
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task RevokeAllTokensForCustomerAsync(int customerId);
        Task<int> DeleteExpiredTokensAsync();
    }
}
