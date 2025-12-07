# Professional Portfolio

My side project to create a profession portfolio on the web. My resume is no longer enough.

Code name: Ghost (Starcraft Unit)

Phase: Implementation of app shells and initial setup of solution

## Project Structure

The solution consists of three projects:

- **DavidKuehn.Portfolio.AppHost** - .NET Aspire orchestration host that manages and coordinates services
- **DavidKuehn.Portfolio.ServiceDefaults** - Shared service configuration and extensions for distributed applications
- **DavidKuehn.Portfolio.WebApi** - ASP.NET Core Web API with API key authentication

## Technology Stack

- **.NET 9** - Application framework
- **ASP.NET Core** - Web API
- **Aspire** - Cloud-native application orchestration
- **Blazor** (planned) - Web UI with static server-side rendering
- **SQL Server** - Primary data store
- **Azure** - Cloud hosting platform
- **Bootstrap** - CSS framework

## Security

The Web API implements API key authentication. The API key is passed via environment variable (`PORTFOLIO_API_KEY`). Additional security enhancements are planned for future phases.

## Prerequisites

- Docker (required for running with Aspire)
- .NET 9 SDK
- (Optional) Dev container support for containerized development

## Running Locally

Docker is required for running this solution locally since it uses Aspire. There is a dev container spec for running within a linux container.

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project src/DavidKuehn.Portfolio.AppHost
```

This will start the Aspire dashboard and orchestrate all services.

## Architecture

For detailed architecture decisions and technology choices, see [ADR.md](ADR.md).

## Development Notes

See [diary.md](diary.md) for project journey and progress updates.
