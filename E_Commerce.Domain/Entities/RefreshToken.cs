
namespace E_Commerce.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        private RefreshToken() { }
        public RefreshToken(string token, int customerId, DateTime expiresAt)
        {
            Token = token;
            CustomerId = customerId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
            IsUsed = false;
        }

        public string Token { get; private set; }
        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public bool IsUsed { get; private set; }

        public void MarkAsUsed() => IsUsed = true;
        public void Revoke() => IsRevoked = true;
        public bool IsActive => !IsRevoked && !IsUsed && DateTime.UtcNow < ExpiresAt;
    }
}
