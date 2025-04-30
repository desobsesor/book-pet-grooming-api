# Book Pet Grooming API

  <p align="center">API services.</p>
    <p align="center">
<a href="https://dotnet.microsoft.com/en-us/" target="_blank"><img src="https://img.shields.io/badge/.NET-8.0-blueviolet" alt=".NET 8.0"></a>
<a href="https://learn.microsoft.com/en-us/aspnet/core/" target="_blank"><img src="https://img.shields.io/badge/ASP.NET%20Core-8.0-blue" alt="ASP.NET Core 8.0"></a>
<a href="https://docs.microsoft.com/en-us/ef/core/" target="_blank"><img src="https://img.shields.io/badge/Entity%20Framework%20Core-8.0-green" alt="Entity Framework Core"></a>
<a href="https://github.com/jbogard/MediatR" target="_blank"><img src="https://img.shields.io/badge/MediatR-10.0.1-ff69b4" alt="MediatR"></a>
<a href="https://fluentvalidation.net/" target="_blank"><img src="https://img.shields.io/badge/FluentValidation-11.5.2-brightgreen" alt="FluentValidation"></a>
<a href="https://automapper.org/" target="_blank"><img src="https://img.shields.io/badge/AutoMapper-12.0.1-orange" alt="AutoMapper"></a>
<a href="https://serilog.net/" target="_blank"><img src="https://img.shields.io/badge/Serilog-3.1.0-lightgrey" alt="Serilog"></a>
<a href="https://swagger.io/" target="_blank"><img src="https://img.shields.io/badge/Swagger%20%2F%20OpenAPI-6.5.0-yellowgreen" alt="Swagger/OpenAPI"></a>
<a href="https://xunit.net/" target="_blank"><img src="https://img.shields.io/badge/xUnit-2.4.2-blue" alt="xUnit"></a>
<a href="https://github.com/moq/moq4" target="_blank"><img src="https://img.shields.io/badge/Moq-4.18.4-lightblue" alt="Moq"></a>
<a href="https://fluentassertions.com/" target="_blank"><img src="https://img.shields.io/badge/FluentAssertions-6.12.0-9cf" alt="FluentAssertions"></a>
</p>


## Description📍

Simple Payment API.

# Professional .NET Application Structure

This project implements a layered architecture following the principles of Clean Architecture and Domain-Driven Design (DDD).

## Project Structure

```
Solution 'BookPetGroomingAPI'
├── src/
│   ├── BookPetGroomingAPI.API                 # Presentation Layer - REST API
│   ├── BookPetGroomingAPI.Application         # Application Layer - Use Cases
│   ├── BookPetGroomingAPI.Domain              # Domain Layer - Entities and Business Rules
│   ├── BookPetGroomingAPI.Infrastructure      # Infrastructure Layer - Concrete Implementations
│   └── BookPetGroomingAPI.Shared              # Shared Components Across Layers
├── tests/
│   ├── BookPetGroomingAPI.UnitTests           # Unit Tests
│   ├── BookPetGroomingAPI.IntegrationTests    # Integration Tests
│   └── BookPetGroomingAPI.FunctionalTests     # Functional Tests
```

## Application Layers

### Domain Layer (BookPetGroomingAPI.Domain)
Contains business entities, repository interfaces, domain events, and business rules. This layer is independent of any external framework or technology.

### Application Layer (BookPetGroomingAPI.Application)
Contains application logic and orchestration. Implements use cases that coordinate the flow of data to and from domain entities, and directs those entities to use their business rules to achieve the objectives of the use case.

### Infrastructure Layer (BookPetGroomingAPI.Infrastructure)
Contains concrete implementations of interfaces defined in the domain and application layers. Includes data access, external services, logging, etc.

### Presentation Layer (BookPetGroomingAPI.API)
Exposes the application's functionality through a REST API. Handles HTTP requests, input validation, and response serialization.

### Shared Components (BookPetGroomingAPI.Shared)
Contains components, utilities, and models that are shared across multiple layers.

## Implemented Patterns

- **Repository Pattern**: To abstract data access
- **Mediator Pattern (CQRS)**: To separate read and write operations
- **Unit of Work**: To manage transactions
- **Dependency Injection**: To decouple components
- **Options Pattern**: To manage configuration
- **Specification Pattern**: For complex queries

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- MediatR
- FluentValidation
- AutoMapper
- Serilog
- Swagger/OpenAPI
- xUnit, Moq, FluentAssertions

## Database Schema 📚

Below is our database schema diagram showing the relationships between tables:

<p align="center">
  <img src="src/BookPetGroomingAPI.Shared/Assets/Images/er-bookpetgrooaming-min.png" alt="Database Schema" width="700"/>
</p>

## SQL Scripts 💡

You can find the database initialization and setup scripts in the `database/schema.sql` directory. These scripts contain:

- Table creation statements
- Initial data seeding
- Stored procedures and functions
- Database user and permission setup

To set up your database, execute the scripts in sequential order as numbered in the filenames.

## Installation 📦

To install and run the project, follow these steps:

1. Clone the repository
```bash
git clone https://github.com/desobsesor/book-pet-grooming-api.git
```
2. Run the maven command
```bash
mvn clean install
```
3. Clone the repository
```bash
dotnet run --project src/BookPetGroomingAPI.API/BookPetGroomingAPI.API.csproj
```

## Built with 🛠️

_Tools and Technologies used_

- [.NET 8](https://dotnet.microsoft.com/en-us/) - Plataforma de desarrollo para aplicaciones modernas y de alto rendimiento
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/) - Framework para construir APIs RESTful y aplicaciones web
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM para acceso a datos relacional
- [MediatR](https://github.com/jbogard/MediatR) - Implementación de patrón Mediator y CQRS
- [FluentValidation](https://fluentvalidation.net/) - Validación de modelos y reglas de negocio
- [AutoMapper](https://automapper.org/) - Mapeo automático de objetos
- [Serilog](https://serilog.net/) - Registro estructurado y logging
- [Swagger/OpenAPI](https://swagger.io/) - Documentación interactiva de la API
- [xUnit](https://xunit.net/) - Framework de pruebas unitarias
- [Moq](https://github.com/moq/moq4) - Mocking framework para pruebas
- [FluentAssertions](https://fluentassertions.com/) - Asserts expresivos para pruebas
- [SQL Server](https://www.microsoft.com/en-us/sql-server) o [PostgreSQL](https://www.postgresql.org/) - Sistemas de gestión de bases de datos relacionales

## Support 🔍

Nest is an MIT-licensed open source project. It can grow thanks to the sponsors and support by the amazing backers. If you'd like to join them, please [read more here](https://docs.nestjs.com/support).

## Versioned 📌

[SemVer](http://semver.org/) is used for versioning. For all versions available.

## Documentation 📖

http://localhost:5000/swagger/index.html

## Author ✒️

_Built by_

- **Yovany Suárez Silva** - _Full Stack Software Engineer_ - [desobsesor](https://github.com/desobsesor)
- Website - [https://portfolio.cds.net.co](https://desobsesor.github.io/portfolio-web/)


## License 📄

This project is under the MIT License - see the file [LICENSE.md](LICENSE.md) for details
