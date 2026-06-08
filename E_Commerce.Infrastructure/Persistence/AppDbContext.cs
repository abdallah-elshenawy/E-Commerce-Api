using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace E_Commerce.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
/*
    Persistence = anything related to storing and retrieving data. Your Infrastructure/Persistence/ folder will contain:

    AppDbContext.cs — the EF Core DbContext
    Repositories/ — concrete implementations of your repo interfaces
    Configurations/ — how your entities map to database tables (column names, constraints, relationships)

Customer (1) ────────── (many) Order
                                  │
                              (many) OrderItem (many) ── (1) Product
 */
