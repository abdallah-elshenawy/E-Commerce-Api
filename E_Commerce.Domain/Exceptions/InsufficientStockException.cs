

namespace E_Commerce.Domain.Exceptions
{
    public class InsufficientStockException : DomainException
    {
        public InsufficientStockException(int productId, int requested, int available) 
            : base($"Product {productId}: requested {requested}, only {available} available.")
        { }
    }
}
