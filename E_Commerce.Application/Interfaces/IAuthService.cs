

using E_Commerce.Application.DTOs.AuthDTOs;

namespace E_Commerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<AuthResponseDto> RegisterAsync(RegisterCustomerDto registerCustomerDto);
    }
}
