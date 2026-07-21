# CLAUDE.md — Project-1 (Enterprise E-Commerce)

> **Read this first, every session.** It carries the hard rules and the current state of the
> work so a fresh session continues exactly as agreed — no re-litigating decisions.

---

## 0. Hard Rules (NEVER break these)

These are non-negotiable guardrails set by the repo owner. They override convenience.

1. **This is a PERSONAL portfolio repo. It is completely separate from any work code.**
   - Remote: `https://github.com/004-rahul/Project-1.git`
   - **NEVER** commit or push this project's work to any Casepoint / work repository or branch.
   - **NEVER** commit any work-repo (Casepoint) code into this repo.

2. **Commit only with the owner's personal identity.**
   - `user.name = Rahul Patel`, `user.email = rahulpatel.bca10@gmail.com` (already set locally in this repo).
   - **NEVER** use a work / Casepoint account to commit here. If the identity ever looks wrong, STOP and tell the owner — do not commit.

3. **Do NOT persist the owner's personal details anywhere.**
   - Do not write the owner's personal / profile / career details into Claude memory, disk, or any cloud.
   - This means: no auto-memory entries about the person, no notes files with personal info.
   - (This is the "don't add logs" rule — it is about *AI memory / saved personal notes*, **not** about the app's own logging feature, which is wanted — see §3.)

4. **Never fake or backdate commit timestamps.** Commits reflect the real date work happened. No inventing a work history.

5. **Do not commit or push unless explicitly told to.**
   - The owner often batches commits to a specific day and does the commit+push himself.
   - Default = build the code and **HOLD** it uncommitted in the working tree. Wait for an explicit "commit and push" (or "commit").
   - When told to commit, prefer small, clean, logical commits — one runnable feature each.

---

## 1. What This Project Is

A **portfolio-grade Enterprise E-Commerce** app — meant to look startup-ready, not college-level.
Two-sided: **customer storefront** + **admin area**, with role-based access.
Backend serves both **MVC pages** (cookie auth) and a **REST API** (`/api/v1`, JWT) so a future
mobile app can reuse the same Application services. One backend, two front doors.

**Tech stack:** ASP.NET Core 9 MVC (no Angular/React), C# / .NET 9, SQL Server + EF Core 9,
clean layered architecture. Planned learning/demo additions: **Redis, RabbitMQ, SignalR,
Firebase (FCM)**, plus JWT + refresh tokens (done) and AI semantic search (later).

**Architecture (dependencies point inward only):**
```
ECommerce.Domain  →  ECommerce.Application  →  ECommerce.Infrastructure  →  ECommerce.Api
```
- Repository + Unit-of-Work pattern.
- Application layer organized by feature module (Catalog / Identity / Ordering).
- Admin is an MVC **Area**; storefront at root; REST controllers under `/api/v1`.

---

## 2. How to Run

```bash
# from repo root
dotnet run --project src/ECommerce.Api
```
- HTTPS: `https://localhost:7003`  ·  HTTP: `http://localhost:5138`
- DB: SQL Server LocalDB `(localdb)\mssqllocaldb`, database `ecommerce_db`.
- On startup the app auto-migrates and seeds **roles (Admin/Customer) + one admin user**.
  - Admin login: `admin@shop.local` / `Admin#12345`
- API tester (Development only): `https://localhost:7003/scalar/v1`
- If HTTPS shows `ERR_HTTP2_INADEQUATE_TRANSPORT_SECURITY`: run `dotnet dev-certs https --trust` once, or use the HTTP profile.
- Bootstrap/jQuery are **vendored locally** in `wwwroot/lib` (CDN is blocked on the corporate network). Keep them vendored.

---

## 3. Working Agreements (how the owner wants features built)

- **Every commit = one complete, runnable feature** the owner can check same-day — with a **UI
  page AND its API**, not API-only. ("If I can't see and run it, it's not done.")
- **UI: clean and simple, mobile-friendly.** The owner is not a full-stack dev — no extra flourish,
  no clutter, but it must look proper and work on phones.
- **Use real database entries, not dummy/hardcoded seed data.** The catalog starts **empty**;
  only roles + the admin user are seeded. Data is created through the app.
- **Exception handling + activity logs are wanted as real features** (basic but proper):
  global exception handling, and activity logs for events like login/logout. (This is the app's
  own logging — allowed and desired. Distinct from Rule §0.3.)
- **Roles drive the UI:** customers see the storefront; admins get a proper left-menu admin
  dashboard. Product statuses are simplified to **Draft / Active / Inactive** (out-of-stock is
  derived from stock quantity, not a separate status).
- Minimal, surgical changes. No new libraries/patterns without the owner's OK.

---

## 4. Current State (update this as work progresses)

**Committed & pushed (all live-verified):** clean layered skeleton; Domain/Application/Infrastructure/Api;
EF Core + Identity; storefront product grid + details (**public** — browse without login); admin Area
(dashboard, products list/create); cookie login/logout with role-based nav; seed = roles + admin (no
dummy products). **REST API v1** (`/api/v1/products`, Scalar, ProblemDetails). **JWT + refresh tokens**
(`/api/v1/auth` register/login/refresh/logout/me; rotation + revocation). **Customer web registration**
with full profile (first/last name, phone, address) + auto-login. **Google sign-in scaffold** (button
shows only when OAuth keys are set; callback creates/links a local account; we don't store Google tokens).

**Nothing currently held** — working tree is clean.

**Next planned steps:**
- Serilog structured logging + global exception handling + login/logout activity logs.
- Admin: product Edit + Categories management.
- Add real Google OAuth keys via user-secrets to activate the Google button (`Authentication:Google:ClientId`/`ClientSecret`); redirect URI `/signin-google`.
- Later — cart (Redis), orders, RabbitMQ, SignalR, Firebase FCM, AI semantic search, Docker Compose, tests.

> `PROGRESS.md` holds the running status log. Keep it and this file's §4 in sync.

---

## 5. Quick Layout

```
src/
  ECommerce.Domain/          Entities, Enums (ProductStatus = Draft/Active/Inactive), Common/BaseEntity
  ECommerce.Application/      Products/, Identity/ (DTOs), Common/Interfaces (services, repos, UoW)
  ECommerce.Infrastructure/   Persistence/ (DbContext, DbInitializer, migrations, repositories),
                              Identity/ (ApplicationUser, JwtTokenService, IdentityService, RefreshToken)
  ECommerce.Api/              Program.cs, Controllers/ (storefront + Api/V1), Areas/Admin/, Views/, wwwroot/
```
