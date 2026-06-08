
namespace E_Commerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        private Customer() { }   
        public Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
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
