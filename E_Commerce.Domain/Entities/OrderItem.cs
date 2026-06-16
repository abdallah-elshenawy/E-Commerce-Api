

using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        private int _productId;
        private int _quantity;
        private decimal _unitPrice;
        private OrderItem() { }
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value >= 1 ? value
                            : throw new DomainException("The product ids start from 1.");
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                Quantity = value >= 1 ? value
                            : throw new DomainException("The quantity cannot be 0.");
            }
        }
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value >= 1 ? value
                            : throw new DomainException("The unit price cannot be 0.");
            }
        }
        public Product Product { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; }
        public OrderItem(int productId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
