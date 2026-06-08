
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.AuthDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;


namespace E_Commerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterCustomerDto registerCustomerDto)
        {
            if (await _unitOfWork.CustomerRepository.GetByEmailAsync(registerCustomerDto.Email) is not null)
                throw new ConflictException("The email is already existed!");

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(registerCustomerDto.Password);
            Customer customer = new Customer(registerCustomerDto.FirstName, registerCustomerDto.LastName, registerCustomerDto.Email);
            customer.SetPassword(hashPassword);

            _unitOfWork.CustomerRepository.Add(customer);
            await _unitOfWork.SaveChangesAsync();

            string newAccessToken = _tokenService.GenerateAccessToken(customer);
            (string token, DateTime expiresAt) generatedRefreshToken = _tokenService.GenerateRefreshToken();


            RefreshToken newRefreshToken = new RefreshToken(generatedRefreshToken.token, customer.Id, generatedRefreshToken.expiresAt);
            _unitOfWork.RefreshTokenRepository.Add(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = generatedRefreshToken.token,
                ExpiresAt = generatedRefreshToken.expiresAt
            };
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            Customer customer = await _unitOfWork.CustomerRepository.GetByEmailAsync(loginDto.Email);
            if (customer is null)
                throw new NotFoundException("Customer", loginDto.Email);

            bool verify = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.PasswordHash);
            if (!verify)
                throw new UnauthorizedException("The password is wrong!");

            string newAccessToken = _tokenService.GenerateAccessToken(customer);
            (string token, DateTime expiresAt) generatedRefreshToken = _tokenService.GenerateRefreshToken();
            RefreshToken refreshToken = new RefreshToken(generatedRefreshToken.token, customer.Id, generatedRefreshToken.expiresAt);

            _unitOfWork.RefreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = generatedRefreshToken.token,
                ExpiresAt = generatedRefreshToken.expiresAt
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            RefreshToken refreshToken = await _unitOfWork.RefreshTokenRepository.GetRefreshTokenByTokenAsync(refreshTokenDto.RefreshToken);
            if (refreshToken is null)
                throw new UnauthorizedException("You are not authorized!");

            if (refreshToken.IsUsed)
            {
                await _unitOfWork.RefreshTokenRepository.RevokeAllTokensForCustomerAsync(refreshToken.CustomerId);
                throw new UnauthorizedException("You are not authorized!");
            }

            if (!refreshToken.IsActive)
                throw new UnauthorizedException("You are not authorized!");

            refreshToken.MarkAsUsed();

            string newAccessToken = _tokenService.GenerateAccessToken(refreshToken.Customer);
            (string token, DateTime expiresAt) newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken(newRefreshToken.token, refreshToken.CustomerId, newRefreshToken.expiresAt);
            _unitOfWork.RefreshTokenRepository.Add(newRefreshTokenEntity);

            await _unitOfWork.SaveChangesAsync();
            return new AuthResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.token,
                ExpiresAt = newRefreshToken.expiresAt
            };
        }
    }
}
