# Project Progress

Running status of the Enterprise E-Commerce build. Updated as features land.

**Last updated:** 2026-07-16

---

## ✅ Done

**Architecture & foundation**
- Clean layered architecture — Domain → Application → Infrastructure → Api (dependencies point inward only), .NET 9

**Domain & data**
- Entities: `Product`, `Category`, `BaseEntity` (auto audit timestamps) + `ProductStatus` enum
- EF Core 9 + SQL Server, Fluent configuration, initial migration (tables, indexes, FK)
- Repository + Unit-of-Work pattern (`IProductRepository`, `ICategoryRepository`, `IUnitOfWork`)
- Auto-migrate + seed on startup (2 categories, 6 products) — a fresh clone just runs, no manual DB steps

**Application layer**
- DTOs (`ProductDto`, `CreateProductRequest`) + `ProductService` (use-case logic, entity↔DTO mapping)

**Web / UI (two-sided, ASP.NET Core MVC + Bootstrap, mobile-friendly)**
- MVC host, responsive layout, Bootstrap + jQuery vendored locally (works offline)
- Storefront: product grid (home) + product detail page
- Admin: manage products — list / create (validated form) / delete
- Clean navigation: Products · Admin

---

## 🚧 Next up
- Serilog structured logging + global exception handling
- Authentication + roles (Admin / Customer) with login / logout activity logs
- Storefront search & filter, product edit

---

## ⬜ Not started
- Shopping cart (Redis-backed) and orders
- Redis (cache-aside, rate limiting)
- RabbitMQ (order events, retry, dead-letter queue)
- SignalR (live order status)
- Firebase Cloud Messaging (push notifications)
- AI semantic product search
- Admin observability: application logs, auth audit, notifications, Redis & RabbitMQ monitors
- Docker Compose (API + SQL Server + Redis + RabbitMQ)
- Unit & integration tests
