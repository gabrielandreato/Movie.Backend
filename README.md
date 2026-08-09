# Movies Backend

Movie/Actor catalog API built with .NET 10, following Clean Architecture and DDD principles.

> **Note:** This project was coded by hand. AI was used only to assist with code review, answer doubts, and help write this documentation.

## Architecture

The solution is split into 4 layers with dependencies pointing inward:

- **Domain** — entities, enums, business invariants. No dependencies on any other project or framework.
- **Application** — services (use cases), DTOs (requests/responses), interfaces for infrastructure concerns. Depends only on Domain.
- **Infra.Data** — EF Core `DbContext`, entity configurations, migrations, and the `MigrationHostedService` that applies pending migrations on startup.
- **Infra.Ioc** — composition root; wires Infra.Data implementations into Application interfaces via DI.
- **API** — controllers, middlewares, and configuration (CORS, Swagger). No business logic.

### Key decisions
- **Clean Architecture**, enforced via project references (outer layers depend on inner ones only).
- **DDD-style entities** — entities encapsulate their own invariants instead of being plain data bags (anemic model).
- **No Repository pattern** — Application services use `DbContext`/`DbSet` directly through injected abstractions, to keep the exercise scope small.
- **Testing split by responsibility**:
  - `tests/Domain.Test` — unit tests for entity/business rules (Domain has no external dependencies, so no mocking needed).
  - `tests/Application.Tests` — unit tests for services, using EF Core InMemory in place of a real database.
  - `tests/Integration.Tests` — end-to-end tests through `WebApplicationFactory`, covering API + DI + configuration wiring.

## Libraries

| Concern | Library |
|---|---|
| Web framework | ASP.NET Core (.NET 10) |
| ORM | Entity Framework Core 10 + Npgsql (PostgreSQL) |
| API docs | Swashbuckle (Swagger UI) over `Microsoft.AspNetCore.OpenApi` |
| Result handling | FluentResults |
| Tests | xUnit, EF Core InMemory, `Microsoft.AspNetCore.Mvc.Testing` |

## Migration & seed strategy

Migrations are managed with EF Core and live in `src/Infra.Data/Data/Migrations`. There's no manual `dotnet ef database update` step required to run the app:

- `MigrationHostedService` (`src/Infra.Data/Data/MigrationHostedService.cs`) runs as an `IHostedService` and calls `Database.MigrateAsync()` on application startup, applying any pending migrations automatically.
- Seed data (e.g. `SeedMovieActorData` migration) is included as a regular EF Core migration, so it's applied the same way, keeping schema and initial data versioned together.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Docker + Docker Compose (to run PostgreSQL, or the whole stack)

## Running the project

### Option A — Docker Compose (API + PostgreSQL)

```bash
docker compose up --build
```

- API available at `http://localhost:8080`, Swagger UI at `http://localhost:8080/swagger`.
- Runs with `ASPNETCORE_ENVIRONMENT=Development` so Swagger is exposed — see simplifications below.

### Option B — API locally, PostgreSQL in Docker

```bash
docker compose up postgres
dotnet run --project src/API
```

Update `ConnectionStrings:DefaultConnection` in `src/API/appsettings.Development.json` if your local Postgres port/credentials differ from the defaults.

### Tests

```bash
dotnet test
```

## Requirements coverage (backend)

### Functional

| Requirement | How it's covered |
|---|---|
| Each movie must be associated with one or more actors | `Movie.Actors` is a navigation collection (`src/Domain/Entities/Movie.cs`), mapped as a many-to-many relationship in EF Core. `MovieController` exposes `POST /movie/{id}:add-actor` and `POST /movie/{id}:remove-actor`, backed by `MovieService.AddActorAsync`/`RemoveActorAsync` (`src/Application/Services/MovieService.cs`), to manage the association. |
| Search movies by title, genre, or actor name | `GET /movie` (`MovieController.Get`) accepts optional `title`, `genre`, and `actorName` query params, translated into composable `IQueryable` filters in `MovieService.GetAsync` — e.g. `query.Where(m => m.Actors.Any(a => EF.Functions.Like(a.Name, $"%{actorName}%")))` for actor name search. |
| Search results displayed on the same page, below the search form | Frontend concern — the backend supports it by returning all matches for a query in a single paginated response (`PagedResponse<MovieResponse>`), so the SPA can render the form and results together without page navigation. |

### Technical

| Requirement | How it's covered |
|---|---|
| Recent version of .NET | All projects target `net10.0` (see any `.csproj`, e.g. `src/API/API.csproj`). |
| SQL Server / SQL Server Express / PostgreSQL | PostgreSQL, via `Npgsql.EntityFrameworkCore.PostgreSQL` (`src/Infra.Data/Infra.Data.csproj`) and the `postgres:16-alpine` image in `docker-compose.yml`. |
| Entity Framework Core as ORM | `ApplicationDbContext` (`src/Infra.Data/Data/ApplicationDbContext.cs`) with code-first migrations in `src/Infra.Data/Data/Migrations`; queries in `MovieService` use EF Core's `DbSet`/`IQueryable` API. |
| Use of Docker | `docker-compose.yml` + `src/API/Dockerfile` build and run the API and PostgreSQL together with `docker compose up --build`. |
| Automated tests (plus) | Three dedicated test projects: `tests/Domain.Test` (entity rules), `tests/Application.Tests` (`MovieService` with EF Core InMemory), `tests/Integration.Tests` (`MovieControllerTests` through `WebApplicationFactory`). |
| Documentation of functionality and implementation approach | This README (architecture, libraries, run instructions) plus this requirements-coverage table. |
| Code comments and documentation in English | Followed throughout the codebase and this document. |

## Simplifications (exercise scope)

- The `Actor` entity/functionality is not fully fleshed out — kept minimal for the exercise.
- No structured logging (e.g. Serilog) — default ASP.NET Core logging is used as-is.
- Only one integration test is applied, using a mocked/in-memory database, to demonstrate the strategy rather than cover it exhaustively.
- Any committed secret values (e.g. the `1234` DB password in `.env` / `appsettings.json`) are throwaway values for this exercise. In a real project these would be gitignored and generated per-environment, not committed.
- `docker-compose.yml` runs the API in `Development` mode to expose Swagger UI easily — not representative of a production setup.
- No authentication/authorization is applied.
