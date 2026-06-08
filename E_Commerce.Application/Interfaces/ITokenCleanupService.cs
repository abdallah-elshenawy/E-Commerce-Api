

namespace E_Commerce.Application.Interfaces
{
    public interface ITokenCleanupService
    {
        Task RemoveExpiredTokensAsync();
    }
}
