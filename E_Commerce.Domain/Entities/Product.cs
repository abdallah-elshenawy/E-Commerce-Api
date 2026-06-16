using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        private string _name;
        private decimal _price;
        private int _stockQuantity;
        private int _categoryId;
        private Product() { }
                             
        public Product(string name, string description, decimal price, int stockQuantity, int categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
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
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value >= 0 ? value
                        : throw new DomainException("The price cannot be 0.");
            }
        }
        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value >= 0 ? value
                        : throw new DomainException("The stock quantity cannot be 0.");
            }
        }
        public string Description { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public int CategoryId { get; private set; }
        public Category Category { get; private set; }

        public void IncreaseStock(int quantity)
        {
            StockQuantity += quantity;
        }
        public void ReduceStock(int quantity)
        {
            if (StockQuantity - quantity < 0)
            {
                throw new InsufficientStockException(Id, quantity, StockQuantity);
            }
            StockQuantity -= quantity;
        }
        public void UpdateDetails(string name, decimal price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
        public void ChangeProductCategory(int newCategoryId)
        {
            CategoryId = newCategoryId;
        }
    }
}
