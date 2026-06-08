using E_Commerce.Domain.Enums;
namespace E_Commerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        private Order() { }  
        public Order(int customerId)
        {
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }

        public int CustomerId { get; private set; }
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
