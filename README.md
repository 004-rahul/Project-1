# Enterprise E-Commerce API

A production-style e-commerce backend built with **ASP.NET Core 9**, designed to demonstrate the
patterns expected of a senior .NET backend engineer: clean layered architecture, secure
authentication, caching, event-driven messaging, containerization, and automated testing — with a
lightweight MVC storefront and an AI-powered product search on top.

> This is a portfolio project. Each feature is developed and committed incrementally.

## Tech stack

| Concern | Technology |
|---|---|
| Runtime / language | .NET 9, C# 13 |
| API | ASP.NET Core Web API (controllers) |
| Frontend | ASP.NET Core MVC (Razor) |
| Database | PostgreSQL + Entity Framework Core 9 |
| Caching | Redis (cache-aside, rate limiting, distributed lock) |
| Messaging | RabbitMQ (events, retry, dead-letter queue) |
| Auth | JWT access tokens + refresh-token rotation |
| Testing | xUnit, Moq, FluentAssertions |
| Packaging | Docker + Docker Compose |
| AI | Semantic product search via embeddings |

## Architecture

Clean, layered dependency flow — dependencies point inward only:

```
                 ┌────────────────────┐        ┌────────────────────┐
   HTTP  ───────▶│  ECommerce.Api     │        │  ECommerce.Web     │
                 │  (Web API)         │        │  (MVC storefront)  │
                 └─────────┬──────────┘        └─────────┬──────────┘
                           │                             │
                           ▼                             ▼
                 ┌───────────────────────────────────────────────┐
                 │            ECommerce.Application               │
                 │  interfaces · DTOs · service contracts         │
                 └───────────────────────┬───────────────────────┘
                                         │
            ┌────────────────────────────┼───────────────────────────┐
            ▼                                                         ▼
  ┌──────────────────────┐                              ┌──────────────────────┐
  │ ECommerce.Domain     │                              │ ECommerce.Infrastructure│
  │ entities · enums     │◀─────────────────────────────│ EF Core · Redis · MQ  │
  └──────────────────────┘                              └──────────────────────┘
```

- **Domain** — business entities and enums. No external dependencies.
- **Application** — use-case interfaces, DTOs, and service contracts. Depends only on Domain.
- **Infrastructure** — EF Core `DbContext`, repositories, Redis, RabbitMQ, and other I/O concerns.
- **Api / Web** — HTTP entry points; wire everything up through dependency injection.

## Feature checklist

- [x] Solution scaffold & layered project structure
- [ ] Product & category domain model
- [ ] EF Core + PostgreSQL persistence
- [ ] Product catalog CRUD API
- [ ] JWT authentication + refresh-token rotation
- [ ] Shopping cart & orders
- [ ] Redis caching (cache-aside) + rate limiting
- [ ] RabbitMQ order events (email/invoice) with retry + DLQ
- [ ] MVC storefront
- [ ] Unit tests
- [ ] AI-powered semantic product search
- [ ] Docker Compose (API + PostgreSQL + Redis + RabbitMQ)

## Getting started

> Prerequisites: [.NET 9 SDK](https://dotnet.microsoft.com/download). PostgreSQL, Redis, and
> RabbitMQ arrive later via Docker Compose.

```bash
# restore & build
dotnet build

# run the API
dotnet run --project src/ECommerce.Api
```

## Project structure

```
EnterpriseECommerce/
├── EnterpriseECommerce.sln
└── src/
    ├── ECommerce.Domain/          # entities, enums
    ├── ECommerce.Application/     # interfaces, DTOs, service contracts
    ├── ECommerce.Infrastructure/  # EF Core, Redis, RabbitMQ
    └── ECommerce.Api/             # Web API controllers
```
