
namespace E_Commerce.Application.Common
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        { 
            
        }
    }
}
