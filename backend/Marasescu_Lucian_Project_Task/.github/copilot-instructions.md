# Copilot Instructions for Device Management System

## Project Overview

This is an **ASP.NET Core 8 Web API** project for managing devices and users.  
It uses **Entity Framework Core 8** with **SQL Server** and exposes a **Swagger UI** for testing.

---

## Architecture

- **Controllers/** — API layer; handle HTTP requests and return responses using DTOs.
- **Data/AppDbContext.cs** — EF Core context; defines `Devices` and `Users` DbSets and model configurations.
- **Entities/** — Plain domain models mapped to database tables (e.g., `Device`, `User`).
- **Dtos/** — Data Transfer Objects used for input/output (never expose raw entities via the API):
  - `*CreateDto` — Used in `POST` requests. All required fields are marked `[Required]` and are non-nullable.
  - `*UpdateDto` — Used in `PATCH` requests. All fields are optional (nullable) to support partial updates.
  - `*ResponseDto` — Returned from all endpoints. Mirrors the entity but without EF navigation concerns.
- **Services/** — Business logic layer (in progress). Scoped services registered in `Program.cs`.
- For each new endpoint use the repository pattern to separate data access logic from controllers.

---

## Coding Conventions

- Use **nullable reference types** (`string?`, `int?`) in `*UpdateDto` classes to indicate all fields are optional.
- Use **non-nullable types** with `= null!` in `*ResponseDto` and entity classes for required string properties.
- Apply **Data Annotations** on DTOs for validation:
  - `[Required]` on mandatory fields in `*CreateDto`.
  - `[MaxLength(n)]` on all string fields.
  - `[Range(min, max)]` on numeric fields (e.g., `RamAmount` is `[Range(1, 128)]`).
- **Do not** place `[Required]` on `*UpdateDto` fields — all updates are partial/optional.
- **Do not** expose `Entity` classes directly in controller responses; always map to a `*ResponseDto`.
- Use **EF Fluent API** in `AppDbContext.OnModelCreating` to configure constraints (do not rely solely on annotations for DB schema).

---

## Naming Conventions

- DTOs: `{Entity}CreateDto`, `{Entity}UpdateDto`, `{Entity}ResponseDto`
- Entities: singular PascalCase (e.g., `Device`, `User`)
- Controllers: plural, suffixed with `Controller` (e.g., `DevicesController`, `UsersController`)
- Services: interface `I{Entity}Service`, implementation `{Entity}Service`
- Routes follow REST conventions: `api/{entity-plural}` (e.g., `api/devices`, `api/users`)

---

## Database

- **SQL Server** is used as the database provider.
- Connection string is configured in `appsettings.json` under `ConnectionStrings:DefaultConnection`.
- Use **EF Core migrations** to manage schema changes:
  ```
  dotnet ef migrations add <MigrationName>
  dotnet ef database update
  ```

---

## Key Dependencies (from .csproj)

| Package | Version |
|---|---|
| `Microsoft.EntityFrameworkCore` | 8.0.10 |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.10 |
| `Microsoft.EntityFrameworkCore.Tools` | 8.0.10 |
| `Swashbuckle.AspNetCore` | 6.6.2 |
