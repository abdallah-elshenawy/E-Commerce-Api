using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class Product : BaseEntity
    {

        private Product() { } // this is for EF Core, to prevent direct instantiation without using the constructor with parameters.
                             
        public Product(string name, string description, decimal price, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        private decimal _price;
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price
        {
            get { return _price; }
            private set
            {
                if (value < 0)
                {
                    throw new DomainException("Price cannot be negative.");
                }
                _price = value;
            }
        }
        public int StockQuantity { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
        public byte[] RowVersion { get; private set; }
        public void IncreaseStock(int quantity)
        {
            if (quantity < 0)
            {
                throw new DomainException("Amount must be positive.");
            }
            StockQuantity += quantity;
        }
        public void ReduceStock(int quantity)
        {
            if (quantity < 0)
            {
                throw new DomainException("Amount must be positive.");
            }
            if (StockQuantity - quantity < 0)
            {
                throw new InsufficientStockException(Id, quantity, StockQuantity);
            }
            StockQuantity -= quantity;
        }
        public void UpdateDetails(string name, decimal price, string description)
        {
            Name = name;
            Price = price > 0 ? price : 0;
            Description = description;
        }

    }
}
