

using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Customer customer);
        (string token, DateTime expiresAt) GenerateRefreshToken();
    }
}
