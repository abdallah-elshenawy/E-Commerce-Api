
namespace E_Commerce.Application.Common
{
    public static class CacheKeys
    {
        public static string Product(int id) => $"products:{id}";
        public const string AllProducts = "products:all";
    }
}
