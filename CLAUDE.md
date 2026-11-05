# Project: Portfolio

Instructions for Claude Code working in this repository. Follow these by default; ask before deviating.

## Stack

- **.NET 10 / C# 14** (target: `net10.0`)
- **Web:** ASP.NET Core Blazor
- **Data:** Dapper
- **Validation:** FluentValidation
- **Observability:** OpenTelemetry
- **API docs:** OpenAPI (built-in)
- **Orchestration:** .NET Aspire
- **Testing:** xUnit + mocks
- **Architecture:** Clean Architecture

## Architecture & structure

- Four layers: Domain, Application, Infrastructure, API. Dependencies point inward. Domain has **zero** external dependencies.
- Match the conventions in existing features before writing new code. Don't introduce a new pattern without asking.

## Conventions

- **Errors:** use the **Result pattern** for expected failures (not-found, validation, conflict). Map to HTTP via `ProblemDetails` at the endpoint. Reserve exceptions for the genuinely unexpected.
- **Async:** every async method takes and passes a `CancellationToken`. Never block on async (`.Result`, `.Wait()`). No `async void` except event handlers.
- **Mapping:** write **explicit** mappings (constructor / `Select`). No mapping library.
- **IDs:** `Guid` keys.
- **Serialization:** use **System.Text.Json** with source-generated contexts for hot paths. Do not add Newtonsoft.Json.
- **Modern C#:** use primary constructors, collection expressions, records, and pattern matching where they genuinely improve the code - not for their own sake.

## Testing

- xUnit with mocks for isolated units. Mock only boundaries you own. Do **not** use `UseInMemoryDatabase` or mock `DbContext` - they hide real bugs.
- Test names: `Method_Scenario_ExpectedResult`. One logical assertion per test. When fixing a bug, first write a failing test that reproduces it.

## Never suggest

- **AutoMapper** - write explicit mappings.
- **Exceptions for control flow** - use the Result pattern for expected outcomes.
- **In-memory database in tests** - use Testcontainers.
- **Business logic in controllers/endpoints** - keep endpoints thin; logic lives in handlers/services.
- **Magic strings/numbers** - use constants, enums, or the Options pattern.
- **Sync-over-async** (`.Result`, `.Wait()`, `.GetAwaiter().GetResult()`) - stay async end to end.
- **`#region` blocks** - if a file needs regions to navigate, split it.
- **Newtonsoft.Json** - use System.Text.Json.
- **Service locator** (`GetService` in business logic) - use constructor injection.
- **Over-abstraction** - no interface with a single implementation "just in case."

## Project setup

- **Central Package Management** - versions live in `Directory.Packages.props`; never pin versions in individual `.csproj` files.
- **Nullable reference types** are enabled solution-wide; treat nullable warnings as errors.
- **Analyzers + `.editorconfig`** are enforced; run `dotnet format` and keep the build warning-free.
- **Global usings** for common namespaces; keep per-file usings minimal.

## Commands

- Build: `dotnet build`
- Test: `dotnet test`
- Run: `dotnet run --project [src/Api]`
- Format: `dotnet format`

## Workflow with Claude

- For non-trivial changes, give a short **plan first** and wait for approval before editing.
- **Don't invent problems** - leave code that's already correct and idiomatic alone.
- Explain each change in one line (what + why).

<!-- Generated with the CLAUDE.md Generator for .NET - thecodeman.net -->
