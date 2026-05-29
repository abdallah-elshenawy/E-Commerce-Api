E-Commerce Order Management API
A production-ready backend API built with .NET 8 following Clean 
Architecture principles with strict dependency rules across four layers.

Key implementations:
- RESTful API with ASP.NET Core covering product catalog, order 
  placement, and customer management
- JWT authentication with refresh token rotation, BCrypt password 
  hashing, and token revocation
- EF Core Code-First with SQL Server, Repository pattern, and 
  Unit of Work for transactional consistency
- Redis distributed caching with graceful degradation and 
  cache invalidation strategy
- Serilog structured logging with daily rolling files and 
  log level overrides per namespace
- FluentValidation for input validation with separation from 
  domain business rules
- Global exception middleware mapping domain exceptions to 
  correct HTTP status codes
- Hangfire recurring background jobs for expired token cleanup
- Domain-driven design with entities enforcing their own 
  business rules (ReduceStock, UpdateDetails, IsActive)

Stack: .NET 8, ASP.NET Core, EF Core, SQL Server, Redis, 
Hangfire, Serilog, FluentValidation, BCrypt
