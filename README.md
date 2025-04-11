# Bidding Management System

A backend system for managing tenders and bidding processes, built with .NET 8, EF Core, and Domain-Driven Design (DDD) architecture.

## Features

- User Management (Registration, Authentication, Role-Based Access Control)
- Tender Creation & Management (CRUD operations, document uploads)
- Bid Submission (Secure storage, validation, modification)
- Tender Evaluation (Scoring, auditing, notifications)

## Architecture

The project follows Clean Architecture and Domain-Driven Design principles:

- **Domain Layer**: Contains all entities, enums, events, value objects, and domain logic
- **Application Layer**: Contains business logic, CQRS commands/queries, DTOs, and interfaces
- **Infrastructure Layer**: Contains implementation of repositories, data access, external services
- **API Layer**: Contains controllers, middleware, and API configurations

## Technology Stack

- Backend: .NET 8 (ASP.NET Core Web API)
- Database: SQL Server with Entity Framework Core
- Authentication: JWT-based authentication and authorization
- API Documentation: Swagger
- Design Patterns: CQRS, Repository, Unit of Work, Mediator

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or another compatible IDE

### Installation

1. Clone the repository

   ```
   git clone https://github.com/jacklusy/TenderManagementSystem.git
   ```

2. Navigate to the project directory

   ```
   cd BiddingManagementSystem
   ```

3. Restore dependencies

   ```
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json` or `appsettings.Development.json`

5. Run the database migrations

   ```
   dotnet ef database update --project BiddingManagementSystem.Infrastructure --startup-project BiddingManagementSystem.API
   ```

6. Run the application
   ```
   dotnet run --project BiddingManagementSystem.API
   ```

## API Documentation

After running the application, you can access the Swagger documentation at:

```
https://localhost:5001/swagger/index.html
```

## Project Structure

```
BiddingManagementSystem/
├── BiddingManagementSystem.API/            # API layer
│   ├── Controllers/                         # API endpoints
│   ├── Extensions/                          # Extension methods
│   ├── Filters/                             # Action filters
│   ├── Middlewares/                         # Custom middlewares
│   ├── Program.cs                           # Entry point
│   └── appsettings.json                     # Configuration
│
├── BiddingManagementSystem.Application/     # Application layer
│   ├── Behaviors/                           # Pipeline behaviors
│   ├── Contracts/                           # Interfaces
│   ├── DTOs/                                # Data Transfer Objects
│   ├── Exceptions/                          # Custom exceptions
│   ├── Features/                            # CQRS features
│   │   ├── Auth/                            # Authentication features
│   │   ├── Bids/                            # Bid features
│   │   ├── Tenders/                         # Tender features
│   │   └── Users/                           # User features
│   └── Mapping/                             # AutoMapper profiles
│
├── BiddingManagementSystem.Domain/          # Domain layer
│   ├── Aggregates/                          # Aggregate roots
│   │   ├── BidAggregate/                    # Bid aggregate
│   │   ├── TenderAggregate/                 # Tender aggregate
│   │   └── UserAggregate/                   # User aggregate
│   ├── Common/                              # Common domain concerns
│   ├── Enums/                               # Enumerations
│   ├── Events/                              # Domain events
│   ├── Exceptions/                          # Domain exceptions
│   ├── Repositories/                        # Repository interfaces
│   └── ValueObjects/                        # Value objects
│
└── BiddingManagementSystem.Infrastructure/  # Infrastructure layer
    ├── Authentication/                      # JWT authentication
    ├── Data/                                # Data access
    │   ├── Configurations/                  # Entity configurations
    │   ├── Context/                         # DbContext
    │   ├── Migrations/                      # EF Core migrations
    │   ├── Repositories/                    # Repository implementations
    │   └── UnitOfWork/                      # Unit of Work
    ├── Services/                            # External services
    └── Storage/                             # File storage
```

## User Roles

The system supports the following user roles:

1. **Admin**: Has full access to all system features
2. **ProcurementOfficer**: Can create and manage tenders
3. **Bidder**: Can view tenders and submit bids
4. **Evaluator**: Can evaluate and score bids

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- This project was developed as part of the Sky Academy – Round 2 (Backend training)
- Instructors: Wedyan Alswiti, Ghaidaa Hammad, and Razan Al Samadi
