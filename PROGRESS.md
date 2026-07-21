# Project Progress

Running status of the Enterprise E-Commerce build. Updated as features land.

**Last updated:** 2026-07-21

---

## ✅ Done

**Architecture & foundation**
- Clean layered architecture — Domain → Application → Infrastructure → Api (dependencies point inward only), .NET 9

**Domain & data**
- Entities: `Product`, `Category`, `BaseEntity` (auto audit timestamps) + `ProductStatus` enum (Draft / Active / Inactive)
- EF Core 9 + SQL Server, Fluent configuration, migrations (catalogue + Identity tables)
- Repository + Unit-of-Work pattern (`IProductRepository`, `ICategoryRepository`, `IUnitOfWork`)
- Auto-migrate + seed on startup (roles + a default admin) — a fresh clone just runs; catalogue starts empty

**Application layer**
- DTOs (`ProductDto`, `CreateProductRequest`) + `ProductService` (use-case logic, entity↔DTO mapping)

**Web / UI (two-sided, ASP.NET Core MVC + Bootstrap, mobile-friendly)**
- MVC host, responsive layout, Bootstrap + jQuery vendored locally (works offline)
- Storefront: product grid (home) + product detail page
- Admin: manage products — list / create (validated form) / delete
- Clean navigation: Products · Admin

**Authentication & access (ASP.NET Core Identity)**
- Customer self-registration (web) — full profile: first/last name, phone, and address (line, city, state, postal code, country); auto-login, Customer role
- **Google sign-in scaffolded** — button appears only when OAuth keys are configured; the callback creates/links a local account entry (we don't store Google's tokens)
- Email/password login & logout (cookie auth), seeded default admin
- Roles: Admin / Customer; the whole `/admin` area is locked to the Admin role
- Role-aware header (Sign in / Register / Logout; Admin link shown only to admins)
- Admin dashboard with a left-sidebar layout
- Storefront is intentionally public (browse without login); auth is for buying/admin

**REST API (one backend, two front doors — web now, mobile later)**
- Versioned JSON API under `/api/v1`, OpenAPI document + Scalar interactive tester, RFC 7807 ProblemDetails errors
- `/api/v1/products` — list + get-by-id (shares the same `IProductService` the web app uses)
- `/api/v1/auth` — register / login / refresh / logout / me
- JWT access tokens + rotating, revocable **refresh tokens** (server-side store; rotation revokes the prior token, logout revokes on demand)

---

## 🚧 Next up
- Serilog structured logging + global exception handling + login/logout activity logs
- Admin: product **Edit** + **Categories** management
- Add real Google OAuth keys (user-secrets) to activate the Google button

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
