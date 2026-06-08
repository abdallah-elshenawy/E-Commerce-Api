using E_Commerce.Application.Interfaces;
using Microsoft.Extensions.Logging;


namespace E_Commerce.Application.Services
{
    public class TokenCleanupService : ITokenCleanupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IUnitOfWork unitOfWork, ILogger<TokenCleanupService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task RemoveExpiredTokensAsync()
        {
            int expiredTokens = await _unitOfWork.RefreshTokenRepository.DeleteExpiredTokensAsync();
            if (expiredTokens > 0)
                _logger.LogInformation("Cleaned up {Count} expired refresh tokens", expiredTokens);
            else
                _logger.LogDebug("Token cleanup job ran — no expired tokens found");
        }
    }
}
