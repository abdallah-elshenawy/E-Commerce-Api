E-Commerce Order Management API — Production-Grade Backend

A fully-featured RESTful API backend demonstrating enterprise .NET 
development practices across 14 structured development iterations.

Architecture: Clean Architecture with strict dependency rules 
(Domain → Application → Infrastructure → API). Domain entities 
enforce their own business invariants. All external concerns 
(database, cache, email) are abstracted behind interfaces and 
injected at the infrastructure layer.

Security: JWT Bearer authentication with refresh token rotation 
and token reuse detection. BCrypt password hashing. Refresh tokens 
stored with revocation support. Optimistic concurrency control via 
SQL Server RowVersion to prevent race conditions on inventory.

Reliability: Global exception middleware mapping domain exceptions 
to HTTP semantics. Redis distributed caching with graceful 
degradation — API continues functioning when cache is unavailable. 
Hangfire recurring job for expired token cleanup. Serilog structured 
logging with daily rolling files and namespace-level filtering.

Quality: FluentValidation separating structural validation from 
domain business rules. xUnit + Moq unit tests covering service 
layer business logic with mocked dependencies.

Stack: .NET 8, ASP.NET Core, EF Core 8, SQL Server, Redis, 
Hangfire, Serilog, AutoMapper, FluentValidation, BCrypt, xUnit, Moq
