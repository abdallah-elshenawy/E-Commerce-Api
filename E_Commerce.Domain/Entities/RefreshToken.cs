
using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        private string _token;
        private int _customerId;
        private DateTime _expiresAt;
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

        public string Token
        {
            get => _token;
            set
            {
                _token = !string.IsNullOrWhiteSpace(value) ? value
                        : throw new DomainException("The token cannot be null or empty.");
                            
            }
        }
        public int CustomerId
        {
            get => _customerId;
            set
            {
                _customerId = value >= 1 ? value
                            : throw new DomainException("The customer ids start from 1.");
            }
        }
        public DateTime ExpiresAt
        {
            get => _expiresAt;
            set
            {
                _expiresAt = value > DateTime.Now ? value
                            : throw new DomainException("The expiration date should exceed current date.");
            }
        }
        public Customer Customer { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public bool IsUsed { get; private set; }

        public void MarkAsUsed() => IsUsed = true;
        public void Revoke() => IsRevoked = true;
        public bool IsActive => !IsRevoked && !IsUsed && DateTime.UtcNow < ExpiresAt;
    }
}
