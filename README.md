feat: implement core e-commerce API with Clean Architecture

- Domain layer: Product, Customer, Order, OrderItem, RefreshToken entities
  with business rules and domain exceptions
- Infrastructure: EF Core Code-First with SQL Server, repository pattern,
  Unit of Work
- Application: ProductService, OrderService, AuthService, CustomerService
  with DTOs and manual mapping
- API: RESTful controllers with JWT authentication, refresh token rotation
- Cross-cutting: FluentValidation, Serilog structured logging,
  Redis caching with graceful degradation, global exception middleware

Stack: .NET 8, EF Core, SQL Server, Redis, Serilog, FluentValidation
