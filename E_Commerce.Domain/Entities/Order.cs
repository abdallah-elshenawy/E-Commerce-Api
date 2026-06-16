using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Exceptions;
namespace E_Commerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        private int _customerId;
        private Order() { }  
        public Order(int customerId)
        {
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;
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
        public Customer Customer { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
        public void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
        }
        public decimal CalculateTotalPrice()
        {
            return OrderItems.Sum(item => item.Quantity * item.UnitPrice);
        }
    }
}
