
using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        private string _name;
        private Category() { }
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = !string.IsNullOrWhiteSpace(value) ? value
                        : throw new DomainException("The name cannot be null or empty.");
            }
        }
        public string Description { get; private set; }
        public ICollection<Product> Products { get; private set; } = new List<Product>();
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}
