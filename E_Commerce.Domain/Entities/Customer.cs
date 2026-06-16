
using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private Customer() { }   
        public Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _lastName = !string.IsNullOrWhiteSpace(value) ? value
                            : throw new DomainException("The first name cannot be null or empty.");
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = !string.IsNullOrWhiteSpace(value) ? value
                            : throw new DomainException("The last name cannot be null or empty.");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = !string.IsNullOrWhiteSpace(value) ? value
                            : throw new DomainException("The email cannot be null or empty.");
            }
        }
        public string PasswordHash { get; private set; }
        public void SetPassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public DateTime CreatedAt { get; private set; }
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    }
}
