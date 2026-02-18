# .NET 10 Upgrade Rationale (2026-02-17)

## Goal
Upgrade the solution from .NET 9 to .NET 10 and move all NuGet dependencies to latest stable releases, while preserving existing runtime behavior.

## Scope
Projects in solution:
- `DavidKuehn.Portfolio.AppHost`
- `DavidKuehn.Portfolio.ServiceDefaults`
- `DavidKuehn.Portfolio.WebApi`

## Why these changes were made

### 1) Target Framework Moniker upgrades
- Changed all project `TargetFramework` values from `net9.0` to `net10.0`.
- Reason: Keep runtime and SDK aligned to current major platform version and receive latest language/runtime/perf/security improvements.
- Risk control: Verified with full solution restore + build after edit.

### 2) Aspire SDK and host package upgrades
- `Aspire.AppHost.Sdk`: `9.0.0-rc.1.24511.1` → `13.1.1`
- `Aspire.Hosting.AppHost`: `9.5.1` → `13.1.1`
- Reason: Existing SDK reference was an RC version; upgraded to latest stable to avoid prerelease drift and ensure supported toolchain.
- Risk control: Build validation of `AppHost` on `net10.0` passed.

### 3) ASP.NET package upgrades
- `Microsoft.AspNetCore.Authentication.JwtBearer`: `9.0.8` → `10.0.3`
- `Microsoft.AspNetCore.OpenApi`: `9.0.9` → `10.0.3`
- Reason: Keep framework-adjacent packages aligned with .NET 10 servicing line and avoid mixed-major dependency graph.
- Risk control: `WebApi` builds successfully on `net10.0`.

### 4) Service defaults and telemetry stack upgrades
- `Microsoft.Extensions.Http.Resilience`: `8.10.0` → `10.3.0`
- `Microsoft.Extensions.ServiceDiscovery`: `9.5.1` → `10.3.0`
- `OpenTelemetry.*`: `1.9.0` → `1.15.0`
- Reason: Unify support matrix on latest stable libraries with active patch cadence and compatibility with modern .NET runtime.
- Risk control: `ServiceDefaults` and downstream projects compile successfully.

## Version selection policy
- Policy used: **latest stable only** (no prerelease versions).
- Source: NuGet flat-container package index endpoints.
- Selection heuristic: highest semantic version that is not preview/rc/beta/alpha.

## Validation performed
Commands:
- `dotnet restore src/DavidKuehn.Portfolio.sln`
- `dotnet build src/DavidKuehn.Portfolio.sln -c Debug`

Outcome:
- Build succeeded for all projects on `net10.0`.
- Existing warnings remain (primarily XML documentation warnings and one nullable warning in API key handler).

## Explicit non-goals in this upgrade
- No feature work.
- No warning cleanup.
- No API behavior changes.
- No architecture refactor.

## Training notes for future AI upgrades
When reproducing this upgrade pattern in other repos:
1. Upgrade TFMs first, then package majors.
2. Prefer stable over prerelease unless explicitly requested.
3. Keep related package families on the same major line.
4. Validate with restore/build before proposing follow-up cleanup.
5. Separate upgrade PR from warning-remediation PR to reduce regression risk.
