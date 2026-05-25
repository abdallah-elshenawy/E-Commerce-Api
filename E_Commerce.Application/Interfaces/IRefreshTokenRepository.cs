
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetRefreshTokenByToken(string token);
    }
}
