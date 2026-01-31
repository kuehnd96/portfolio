# .NET 9 to .NET 10 Migration Plan

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Package Update Reference](#package-update-reference)
5. [Project-by-Project Migration Plans](#project-by-project-migration-plans)
6. [Breaking Changes & API Migration](#breaking-changes--api-migration)
7. [Testing & Validation Strategy](#testing--validation-strategy)
8. [Risk Management](#risk-management)
9. [Complexity & Effort Assessment](#complexity--effort-assessment)
10. [Source Control Strategy](#source-control-strategy)
11. [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario
Upgrade all projects in the David Kuehn Portfolio solution from **.NET 9.0** to **.NET 10.0 (Long Term Support)**.

### Current State
- **Total Projects**: 3 projects (all require upgrade)
- **Current Framework**: All projects target net9.0
- **Project Types**: 
  - 1 DotNetCoreApp (AppHost)
  - 1 AspNetCore (WebApi)
  - 1 ClassLibrary (ServiceDefaults)
- **Total Lines of Code**: 338 LOC
- **Dependency Structure**: Simple linear chain (AppHost ? WebApi ? ServiceDefaults)

### Target State
- **Target Framework**: All projects migrate to net10.0
- **NuGet Packages**: 7 of 10 packages require updates (70%)
- **Code Changes**: 6 source incompatibilities identified (all in WebApi project)
- **Estimated Code Impact**: ~6 LOC changes (1.8% of codebase)

### Selected Strategy: All-At-Once Strategy

**Rationale**:
- ? Small solution (3 projects)
- ? Simple, linear dependency structure (no cycles, no high-risk relationships)
- ? All projects classified as Low difficulty
- ? Minimal code changes required (6 LOC)
- ? No binary incompatibilities
- ? Complete solution is testable as a unit

**Approach**: All projects will be upgraded simultaneously in a single atomic operation. This eliminates multi-targeting complexity, enables unified testing, and delivers the fastest upgrade timeline for this small, simple solution.

### Key Metrics
| Metric | Value | Impact |
| :--- | :---: | :--- |
| Projects to upgrade | 3 | Low |
| Packages to upgrade | 7 | Medium |
| Source incompatibilities | 6 | Medium |
| Estimated LOC changes | 6+ | Low |
| Dependency cycles | 0 | Low |
| Complexity rating | ?? Low | Enables All-At-Once approach |

### Critical Issues
- **JWT Bearer API Changes**: 4 source incompatibilities in WebApi related to JwtBearerDefaults and JwtBearerExtensions (see Breaking Changes section for details)

---

## Migration Strategy

### Approach: All-At-Once Atomic Upgrade

**Strategy**: All projects (ServiceDefaults, WebApi, AppHost) will be upgraded to .NET 10.0 simultaneously in a single coordinated operation. There are no intermediate states or phases—all changes are applied together, the entire solution is built, and tests are executed as a complete unit.

**Justification**:
- Small solution size (3 projects, 338 LOC total)
- Simple linear dependency structure with no cycles
- Low complexity across all projects (all rated Low difficulty)
- Minimal code changes required (6 LOC)
- No binary incompatibilities detected
- Complete solution is testable in one build pass

### Execution Model

**Single Atomic Operation** consisting of:
1. **Update all project files** - Change TargetFramework in all 3 projects from net9.0 to net10.0
2. **Update all NuGet packages** - Apply all 7 package upgrades across all projects
3. **Restore dependencies** - Run dotnet restore to resolve packages
4. **Build entire solution** - Compile all projects together
5. **Fix compilation errors** - Address breaking changes discovered during build
6. **Rebuild and verify** - Ensure solution builds with 0 errors

**No Intermediate States**: Unlike incremental strategies, there are no pauses or phase boundaries. All projects move to .NET 10.0 in a single pass.

### Parallel vs Sequential

**Sequential Build Order** (enforced by project dependencies):
1. First: ServiceDefaults.csproj builds (no dependencies)
2. Then: WebApi.csproj builds (depends on ServiceDefaults)
3. Finally: AppHost.csproj builds (depends on WebApi)

This order is automatic—the build system respects project dependencies.

### Package Update Strategy

**All packages updated in single operation**:
- 7 packages with recommended upgrades applied simultaneously
- 3 compatible packages optionally upgraded to 1.15.0 for consistency (OpenTelemetry packages)

See §Package Update Reference for complete list.

### Testing Model

**Two-phase testing after atomic upgrade**:
1. **Build Validation** - Solution compiles with 0 errors and warnings
2. **Test Execution** - All test projects execute and pass

No intermediate testing between projects—validation happens after all changes are complete.

---

## Detailed Dependency Analysis

### Dependency Graph

```
AppHost (DotNetCoreApp)
    ?
WebApi (AspNetCore) 
    ?
ServiceDefaults (ClassLibrary)
```

**Legend:**
- No circular dependencies
- Linear chain (leaf ? root)
- All projects are SDK-style format

### Dependency Ordering for Upgrade

Since all projects are upgraded simultaneously in All-At-Once strategy, there is no sequential ordering needed. However, the logical dependency chain for reference:

**Phase 1 (All Projects - Atomic Operation)**
1. DavidKuehn.Portfolio.ServiceDefaults (leaf node - no dependencies)
2. DavidKuehn.Portfolio.WebApi (depends on ServiceDefaults)
3. DavidKuehn.Portfolio.AppHost (depends on WebApi)

All three projects are updated in a single coordinated operation, not sequentially.

### Dependency Summary

| Project | Dependencies | Dependants | Migration Order |
| :--- | :---: | :---: | :--- |
| DavidKuehn.Portfolio.ServiceDefaults | 0 (leaf) | 1 (WebApi) | Part of atomic upgrade |
| DavidKuehn.Portfolio.WebApi | 1 (ServiceDefaults) | 1 (AppHost) | Part of atomic upgrade |
| DavidKuehn.Portfolio.AppHost | 1 (WebApi) | 0 (root) | Part of atomic upgrade |

### Critical Path Analysis

**No sequential critical path exists** - All projects have Low complexity and minimal impact. The solution is simple enough that all projects can be upgraded together without intermediate checkpoints.

---

## Package Update Reference

All packages are updated simultaneously during the atomic upgrade operation.

### NuGet Packages - Upgrade Required

| Package | Current | Target | Project(s) | Reason |
| :--- | :---: | :---: | :--- | :--- |
| **Aspire.Hosting.AppHost** | 9.5.1 | 13.1.0 | AppHost | Framework compatibility (.NET 10) |
| **Microsoft.AspNetCore.Authentication.JwtBearer** | 9.0.8 | 10.0.2 | WebApi | Framework alignment (.NET 10) - *Breaking changes expected* |
| **Microsoft.AspNetCore.OpenApi** | 9.0.9 | 10.0.2 | WebApi | Framework alignment (.NET 10) |
| **Microsoft.Extensions.Http.Resilience** | 8.10.0 | 10.2.0 | ServiceDefaults | Framework compatibility (.NET 10) |
| **Microsoft.Extensions.ServiceDiscovery** | 9.5.1 | 10.2.0 | ServiceDefaults | Framework compatibility (.NET 10) |
| **OpenTelemetry.Instrumentation.AspNetCore** | 1.9.0 | 1.15.0 | ServiceDefaults | Version alignment with ecosystem |
| **OpenTelemetry.Instrumentation.Http** | 1.9.0 | 1.15.0 | ServiceDefaults | Version alignment with ecosystem |

### NuGet Packages - Compatible (Optional Update)

The following packages are compatible with .NET 10 but are candidates for version alignment:

| Package | Current | Recommended | Project(s) | Reason |
| :--- | :---: | :---: | :--- | :--- |
| **OpenTelemetry.Exporter.OpenTelemetryProtocol** | 1.9.0 | 1.15.0 | ServiceDefaults | Version consistency with other OpenTelemetry packages |
| **OpenTelemetry.Extensions.Hosting** | 1.9.0 | 1.15.0 | ServiceDefaults | Version consistency with other OpenTelemetry packages |
| **OpenTelemetry.Instrumentation.Runtime** | 1.9.0 | 1.15.0 | ServiceDefaults | Version consistency with other OpenTelemetry packages |

**Recommendation**: Upgrade all OpenTelemetry packages to 1.15.0 together for consistency and to ensure all telemetry instrumentation uses compatible versions.

### Projects Affected by Package Updates

| Project | Package Count | Updates Required |
| :--- | :---: | :--- |
| **DavidKuehn.Portfolio.AppHost** | 1 | 1 (Aspire.Hosting.AppHost) |
| **DavidKuehn.Portfolio.WebApi** | 2 | 2 (JWT Bearer, OpenApi) |
| **DavidKuehn.Portfolio.ServiceDefaults** | 7 | 5 recommended + 3 optional = 8 total |

### Update Locations

All package references are located in `<PackageReference>` elements within the `.csproj` project files:
- `src/DavidKuehn.Portfolio.AppHost/DavidKuehn.Portfolio.AppHost.csproj`
- `src/DavidKuehn.Portfolio.WebApi/DavidKuehn.Portfolio.WebApi.csproj`
- `src/DavidKuehn.Portfolio.ServiceDefaults/DavidKuehn.Portfolio.ServiceDefaults.csproj`

---

## Project-by-Project Migration Plans

All three projects are upgraded simultaneously as part of the atomic operation. Details below describe the scope of changes for each project.

---

### Project 1: DavidKuehn.Portfolio.ServiceDefaults

**Type**: ClassLibrary  
**Current Framework**: net9.0  
**Target Framework**: net10.0  
**Files**: 1 file (119 LOC)  
**Complexity**: ?? Low

#### Current State
- **Project**: `src/DavidKuehn.Portfolio.ServiceDefaults/DavidKuehn.Portfolio.ServiceDefaults.csproj`
- **Target Framework**: net9.0
- **Dependencies**: None (leaf project)
- **Dependants**: WebApi project
- **NuGet Packages**: 7 packages (5 need upgrade, 3 optional upgrades)

#### Target State
- **Target Framework**: net10.0
- **API Compatibility**: 102/102 APIs compatible ? (0 breaking changes expected)

#### Migration Steps

**1. Update TargetFramework**
- Change `<TargetFramework>net9.0</TargetFramework>` ? `<TargetFramework>net10.0</TargetFramework>`

**2. Update NuGet Packages**
- Microsoft.Extensions.Http.Resilience: 8.10.0 ? 10.2.0
- Microsoft.Extensions.ServiceDiscovery: 9.5.1 ? 10.2.0
- OpenTelemetry.Instrumentation.AspNetCore: 1.9.0 ? 1.15.0
- OpenTelemetry.Instrumentation.Http: 1.9.0 ? 1.15.0
- *Optional*: OpenTelemetry.Exporter.OpenTelemetryProtocol: 1.9.0 ? 1.15.0
- *Optional*: OpenTelemetry.Extensions.Hosting: 1.9.0 ? 1.15.0
- *Optional*: OpenTelemetry.Instrumentation.Runtime: 1.9.0 ? 1.15.0

**3. Expected Build Result**
- ? Should build without errors
- ? Should build without warnings
- ?? No breaking API changes expected

#### Testing
- No unit tests in this project (utility library)
- Integration testing handled by dependent projects (WebApi)

---

### Project 2: DavidKuehn.Portfolio.WebApi

**Type**: AspNetCore  
**Current Framework**: net9.0  
**Target Framework**: net10.0  
**Files**: 10 files (203 LOC)  
**Complexity**: ?? Low (despite API changes)

#### Current State
- **Project**: `src/DavidKuehn.Portfolio.WebApi/DavidKuehn.Portfolio.WebApi.csproj`
- **Target Framework**: net9.0
- **Dependencies**: ServiceDefaults
- **Dependants**: AppHost
- **NuGet Packages**: 2 packages (both need upgrade)
- **API Compatibility Issues**: 6 source incompatibilities (all related to JWT Bearer authentication)

#### Target State
- **Target Framework**: net10.0
- **Expected Code Changes**: 6 LOC modifications (primarily JWT Bearer imports/method calls)

#### Migration Steps

**1. Update TargetFramework**
- Change `<TargetFramework>net9.0</TargetFramework>` ? `<TargetFramework>net10.0</TargetFramework>`

**2. Update NuGet Packages**
- Microsoft.AspNetCore.Authentication.JwtBearer: 9.0.8 ? 10.0.2
- Microsoft.AspNetCore.OpenApi: 9.0.9 ? 10.0.2

**3. Address Breaking Changes**
See §Breaking Changes & API Migration section for detailed fixes. Summary:
- JWT Bearer authentication import changes
- JwtBearerDefaults class relocation
- Extension method signature changes

**4. Expected Build Result**
- ?? Initial build will fail with source incompatibility errors (6 errors related to JWT Bearer)
- ? After fixes applied, builds without errors
- ? Builds without warnings

#### Files Affected
- Configuration/authentication setup files (identified in assessment as having incidents)

#### Testing
- Execute unit tests for authentication/authorization logic
- Execute integration tests for API endpoints
- Manual verification of JWT authentication flow

---

### Project 3: DavidKuehn.Portfolio.AppHost

**Type**: DotNetCoreApp  
**Current Framework**: net9.0  
**Target Framework**: net10.0  
**Files**: 1 file (16 LOC)  
**Complexity**: ?? Low

#### Current State
- **Project**: `src/DavidKuehn.Portfolio.AppHost/DavidKuehn.Portfolio.AppHost.csproj`
- **Target Framework**: net9.0
- **Dependencies**: WebApi
- **Dependants**: None (root project)
- **NuGet Packages**: 1 package (needs upgrade)
- **API Compatibility**: 28/28 APIs compatible ? (0 breaking changes expected)

#### Target State
- **Target Framework**: net10.0

#### Migration Steps

**1. Update TargetFramework**
- Change `<TargetFramework>net9.0</TargetFramework>` ? `<TargetFramework>net10.0</TargetFramework>`

**2. Update NuGet Packages**
- Aspire.Hosting.AppHost: 9.5.1 ? 13.1.0

**3. Expected Build Result**
- ? Should build without errors
- ? Should build without warnings
- ?? No breaking API changes expected

#### Testing
- Host application verification (application starts correctly)
- Service integration testing (WebApi is properly hosted)
- Configuration verification

---

## Breaking Changes & API Migration

### Summary

The .NET 9 ? .NET 10 upgrade introduces **6 source incompatibilities**, all isolated to the WebApi project's JWT Bearer authentication setup. These are manageable changes affecting ~6 lines of code.

### Breaking Change Details

#### 1. JWT Bearer Authentication Schema Migration

**Affected API**: `Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults`  
**Occurrences**: 2 references  
**Category**: Source Incompatible  
**Impact**: Requires code updates

**Issue Description**:
The `JwtBearerDefaults` class has been reorganized in Microsoft.AspNetCore.Authentication.JwtBearer 10.0.2. The `AuthenticationScheme` constant may have moved or changed access pattern.

**Migration Path**:
1. Locate all references to `JwtBearerDefaults.AuthenticationScheme`
2. Check if constant has moved to a new namespace or class
3. Update references to use the new location
4. Verify authentication scheme is correctly registered in dependency injection

**Example Code Pattern**:
```csharp
// Old pattern (may fail)
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(...);

// Likely new pattern
// Verify exact location in package documentation
```

**Remediation**: Update import statements and references after package upgrade; verify during build.

---

#### 2. JWT Bearer Extension Method Changes

**Affected API**: `Microsoft.Extensions.DependencyInjection.JwtBearerExtensions`  
**Occurrences**: 1 reference  
**Category**: Source Incompatible  
**Impact**: Requires code updates

**Issue Description**:
The `AddJwtBearer()` extension method signature or namespace may have changed in version 10.0.2.

**Migration Path**:
1. Locate `AddJwtBearer(AuthenticationBuilder)` method calls
2. Verify the method signature remains compatible
3. Check if additional parameters are required or optional parameters have changed
4. Update implementation if necessary

**Remediation**: Verify method signature during build; update calls as needed.

---

### Migration Instructions

**When**: During the atomic upgrade, after all NuGet packages are updated

**How**:
1. Build the solution after framework and package updates
2. Compiler will report source incompatibility errors with line numbers
3. For each error:
   - Examine the error message and line number
   - Consult JWT Bearer 10.0.2 documentation or IntelliSense
   - Apply the suggested fix (usually an import change or method update)
4. Rebuild to verify fixes

**Expected Errors**:
```
error CS1929: 'object' does not contain a definition for 'JwtBearerDefaults' 
            and no accessible extension method 'JwtBearerDefaults' 
            accepting a first argument of type 'object' could be found
```

**Common Fixes**:
- Update namespace import for `JwtBearerDefaults`
- Verify correct extension method is being called
- Check parameter order/types if method signature changed

---

### No Other Breaking Changes Expected

- **ServiceDefaults**: All 102 APIs compatible ?
- **AppHost**: All 28 APIs compatible ?
- **Other packages**: No binary incompatibilities detected

The JWT Bearer changes are localized and straightforward to address.

---

## Testing & Validation Strategy

### Multi-Level Validation Approach

After the atomic upgrade completes and all code changes are fixed, validation occurs in two sequential phases:

#### Phase 1: Build Validation

**Objective**: Verify solution compiles and all references are resolved

**Actions**:
1. Run `dotnet restore` to resolve all NuGet packages to their upgraded versions
2. Run `dotnet build` for the entire solution
3. Verify 0 compilation errors
4. Verify 0 compiler warnings
5. Confirm all projects build in correct dependency order:
   - ServiceDefaults first (no dependencies)
   - WebApi second (depends on ServiceDefaults)
   - AppHost third (depends on WebApi)

**Success Criteria**:
- ? `dotnet build` completes successfully
- ? No error messages
- ? No warning messages
- ? All assembly outputs generated

---

#### Phase 2: Test Execution

**Objective**: Verify functionality is preserved after framework/package upgrades

**Test Projects to Execute**:
*(Note: Assessment indicates 0 test projects; if test projects exist, they should be executed here)*

**Smoke Tests** (if no formal test projects exist):
1. **WebApi Project**
   - Verify API application hosts correctly
   - Verify JWT authentication can be initialized
   - Test at least one authenticated endpoint request

2. **AppHost Project**
   - Verify Aspire host application starts
   - Verify service discovery for WebApi works
   - Verify distributed application configuration loads

3. **ServiceDefaults Project**
   - Verify telemetry initialization works
   - Verify service collection extensions register correctly

**Manual Verification** (for production readiness):
- [ ] Application starts without runtime errors
- [ ] Endpoints respond to HTTP requests
- [ ] JWT authentication flow works end-to-end
- [ ] No performance degradation observed
- [ ] Logging/telemetry captures events correctly

---

### Build Order and Dependencies

The build process respects project dependencies:

```
1. ServiceDefaults.csproj builds
   ?
2. WebApi.csproj builds (after ServiceDefaults succeeds)
   ?
3. AppHost.csproj builds (after WebApi succeeds)
```

If any project fails to build, dependent projects will not build.

---

### Testing Checklist

**Before marking upgrade complete**:

| Item | Status | Notes |
| :--- | :---: | :--- |
| ServiceDefaults builds without errors | [ ] | No code changes expected |
| WebApi builds without errors | [ ] | JWT Bearer fixes required |
| AppHost builds without errors | [ ] | No code changes expected |
| Solution builds with 0 warnings | [ ] | All projects |
| Application can start | [ ] | Manual or integration test |
| JWT authentication works | [ ] | Key functionality test |

---

### Rollback Procedure (if needed)

If testing reveals critical issues:

1. **Revert to previous branch state** (git checkout -- .)
2. **Return to .NET 9** using version control history
3. **Investigate specific error** before retrying
4. **Document issue** for reference

(Note: All changes are on the upgrade branch, so main branch remains unaffected.)

---

## Risk Management

### Risk Assessment Summary

| Risk Category | Risk Level | Impact | Likelihood | Mitigation |
| :--- | :---: | :--- | :---: | :--- |
| JWT Bearer API Changes | Medium | High - breaks authentication | High (6 APIs affected) | Follow breaking changes guide §3; compile-time detection |
| Large package version jumps | Low | Medium - potential behavioral changes | Medium (Aspire 9?13) | Test authentication & hosting thoroughly |
| Unexpected breaking changes | Low | High - unknown impact | Low (thorough analysis done) | Build/test process will catch issues |
| Dependency resolution | Low | Medium - missing packages | Low (simple dependency chain) | dotnet restore validates all packages |
| Deployment issues | Low | High - service disruption | Low (compatible versions selected) | Test in staging first |

### High-Risk Areas

#### 1. JWT Bearer Authentication Migration
**Risk**: JWT Bearer package upgrade from 9.0.8 ? 10.0.2 introduces API changes  
**Severity**: Medium (breaks build but easy to fix)  
**Detection**: Compile-time errors will immediately identify all affected code  
**Mitigation**:
- Review breaking changes section (§6) before building
- Address compiler errors systematically
- Test authentication flow after fixes applied
- Verify tokens are accepted correctly

**Likelihood of Issue**: High (6 source incompatibilities identified)  
**Likelihood of Resolution**: Very High (changes are straightforward)

---

#### 2. Large Aspire Version Jump
**Risk**: Aspire.Hosting.AppHost 9.5.1 ? 13.1.0 is a major version jump  
**Severity**: Low-Medium (potential behavioral changes)  
**Mitigation**:
- Review Aspire 13.x release notes
- Test application hosting thoroughly
- Verify service discovery still works
- Check for configuration changes needed

**Likelihood of Issue**: Low (but possible)

---

### No Critical Blockers Identified

? **No binary incompatibilities** - All issues are source-level  
? **No circular dependencies** - Simple linear chain eliminates ordering issues  
? **No deprecated packages** - Assessment flagged Aspire as potentially deprecated, but updated version (13.1.0) is available  
? **No security vulnerabilities** - No CVEs or security issues blocking upgrade  

### Contingency Plans

**If Build Fails**:
1. Examine error messages carefully
2. Consult §6 Breaking Changes for JWT Bearer issues
3. Update imports/method calls as needed
4. Rebuild to identify next issue

**If Tests Fail**:
1. Identify which component failed (WebApi, AppHost, ServiceDefaults)
2. Review corresponding project migration section (§5)
3. Check if new behavior is expected or a regression
4. Address either by code change or reconfiguration

**If Authentication Breaks**:
1. Verify JWT Bearer package version matches target (10.0.2)
2. Check imports match updated namespace structure
3. Verify `AddJwtBearer()` call matches new signature
4. Test with sample JWT token

**If Aspire Hosting Fails**:
1. Check for configuration changes in Aspire 13.x
2. Verify service names/references are correct
3. Review environment variable handling
4. Check service discovery configuration

---

### Risk Mitigation Priorities

1. **Highest Priority**: Fix JWT Bearer compilation errors (blocking build)
2. **High Priority**: Verify authentication flow works (functional test)
3. **Medium Priority**: Test Aspire hosting behavior (deployment test)
4. **Lower Priority**: Performance validation (optimization, not blocking)

---

### Rollback Strategy

All changes are isolated to the `upgrade-to-NET10` branch (or kept on current branch per your direction).

**If Upgrade Fails Irreparably**:
```
git checkout main
# Returns to pre-upgrade state without affecting main branch
```

**If Issues Found Post-Merge**:
```
git revert <commit-hash>
# Reverts the upgrade commit while maintaining history
```

---

## Complexity & Effort Assessment

### Overall Solution Complexity

**Classification**: ?? **LOW COMPLEXITY**

**Justification**:
- Tiny codebase (338 LOC total)
- Simple linear dependency structure (no cycles)
- All projects rated Low difficulty
- Minimal code changes required (6 LOC)
- Straightforward API changes (JWT Bearer only)
- No architectural refactoring needed

**This is one of the simplest .NET upgrades possible.**

---

### Per-Project Complexity Ratings

| Project | Complexity | Rationale | Estimated Effort | Risk |
| :--- | :---: | :--- | :---: | :--- |
| **ServiceDefaults** | ?? Low | 0 breaking changes, 0 code changes | Minimal | Very Low |
| **WebApi** | ?? Low | 6 source incompatibilities but all in authentication setup | Low | Low |
| **AppHost** | ?? Low | 0 breaking changes, 0 code changes | Minimal | Very Low |

### Effort Breakdown

#### ServiceDefaults - Estimated Effort: **5-10 minutes**
- **Project file update**: < 1 minute (change TargetFramework)
- **Package updates**: < 2 minutes (5-8 PackageReference version bumps)
- **Code changes**: 0 minutes (no code changes)
- **Testing**: 5 minutes (verify compilation)

#### WebApi - Estimated Effort: **15-30 minutes**
- **Project file update**: < 1 minute (change TargetFramework)
- **Package updates**: < 2 minutes (2 PackageReference version bumps)
- **Code changes**: 10-15 minutes (JWT Bearer import/method fixes)
- **Testing**: 5 minutes (verify authentication flow)

#### AppHost - Estimated Effort: **5-10 minutes**
- **Project file update**: < 1 minute (change TargetFramework)
- **Package updates**: < 2 minutes (1 PackageReference version bump)
- **Code changes**: 0 minutes (no code changes)
- **Testing**: 5 minutes (verify hosting)

#### **Total Estimated Effort: 25-50 minutes**

**Note**: Estimates are for execution. Planning, code review, and validation add additional time depending on your process.

---

### Complexity Factors

**Simplifying Factors** ?:
- No multi-targeting needed
- Small codebase limits scope
- Simple dependency graph
- No Entity Framework migrations
- No configuration system changes
- No middleware registration changes
- All SDK-style projects (simpler file format)

**Complicating Factors** ??:
- Aspire version jump is large (9?13)
- JWT Bearer API changes require investigation
- No test projects identified (harder to validate)

**Overall**: Complicating factors are easily manageable.

---

### Effort Comparison: All-At-Once vs Incremental

**All-At-Once** (Selected Strategy):
- Single unified build pass
- Single test pass
- One source control commit
- Faster overall (25-50 min)
- Higher coordination overhead (none for this size)

**Incremental** (Not selected):
- Three separate builds (one per project)
- Three test cycles
- Multiple commits
- Longer overall (40-60 min)
- Better isolation (unnecessary for this size)

**Verdict**: All-At-Once is optimal for this solution size and complexity.

---

### Resource Requirements

**Skill Levels**:
- ? Junior developer with guidance: Can execute with reference to this plan
- ? Mid-level developer: Can execute independently
- ? Senior developer: Can execute and troubleshoot any issues

**Tools Required**:
- Visual Studio 2024+ (preferred) or VS Code
- .NET 10 SDK installed (validation step included)
- Git for source control
- NuGet package manager

**Estimated Team Time**:
- Execution: 30-60 minutes
- Code review: 15-30 minutes
- Testing: 10-20 minutes
- **Total**: 55-110 minutes (less than 2 hours)

---

### Success Probability

**Expected Success Rate**: ?? **95%+**

**Basis**:
- Assessment is thorough and specific
- No unknown blockers identified
- Small scope limits variables
- API changes are well-understood
- All-At-Once approach aligns with solution characteristics

**Why Not 100%**:
- Aspire version jump (9?13) could have surprises
- Runtime behavioral changes always possible
- Environment-specific issues (configuration, permissions)

**Contingency**: If issues arise, rollback is simple and fast.

---

## Source Control Strategy

### Branch Strategy

**Upgrade Branch**: `193_NET10Upgrade` (current branch, reused as upgrade target)

**Rationale**: 
- You provided the existing branch to be used for the upgrade
- Avoids creating an additional branch
- Simplifies integration when ready for merge

### Commit Strategy: Single Atomic Commit

**Recommendation**: Create **one unified commit** containing all upgrade changes

**Commit Structure**:
```
Commit: "Upgrade to .NET 10.0"
??? All 3 project files (.csproj) with TargetFramework updated
??? All NuGet package version references updated
??? All code changes to fix breaking changes (JWT Bearer)
??? Verification: Solution builds with 0 errors
```

**Commit Message**:
```
Upgrade to .NET 10.0

- Update all projects from net9.0 to net10.0
- Upgrade 7 NuGet packages to .NET 10 compatible versions
  - Aspire.Hosting.AppHost: 9.5.1 ? 13.1.0
  - Microsoft.AspNetCore.Authentication.JwtBearer: 9.0.8 ? 10.0.2
  - Microsoft.AspNetCore.OpenApi: 9.0.9 ? 10.0.2
  - Microsoft.Extensions.Http.Resilience: 8.10.0 ? 10.2.0
  - Microsoft.Extensions.ServiceDiscovery: 9.5.1 ? 10.2.0
  - OpenTelemetry.Instrumentation.AspNetCore: 1.9.0 ? 1.15.0
  - OpenTelemetry.Instrumentation.Http: 1.9.0 ? 1.15.0
- Optional: Upgrade OpenTelemetry packages for version consistency
  - OpenTelemetry.Exporter.OpenTelemetryProtocol: 1.9.0 ? 1.15.0
  - OpenTelemetry.Extensions.Hosting: 1.9.0 ? 1.15.0
  - OpenTelemetry.Instrumentation.Runtime: 1.9.0 ? 1.15.0
- Fix JWT Bearer source incompatibilities in WebApi

All projects build with 0 errors and 0 warnings.
```

### Why Single Commit?

**Advantages for All-At-Once Upgrade**:
- ? Represents atomic operation (all or nothing)
- ? Easy to review as single logical unit
- ? Simplifies rollback (one commit to revert)
- ? Clear Git history (one entry per upgrade)
- ? Matches the upgrade approach (simultaneous changes)

**Disadvantages**: None for small solutions like this

---

### Merge Strategy

**When Ready to Integrate to Main**:

**Option 1: Merge Commit** (Recommended)
```
git checkout main
git merge --no-ff upgrade-to-NET10
# Creates merge commit that preserves branch history
```

**Option 2: Fast-Forward Merge**
```
git checkout main
git merge upgrade-to-NET10
# Linear history (no merge commit)
```

**Option 3: Squash Merge** (If multiple intermediate commits)
```
git checkout main
git merge --squash upgrade-to-NET10
# Combines all commits into single commit
```

**Recommendation**: Use Option 1 (Merge Commit) to preserve:
- Complete branch history
- Clear record of when upgrade occurred
- Ability to track upgrade evolution if needed

---

### Code Review Checklist

Before merging to main, verify:

- [ ] All 3 project files updated to net10.0
- [ ] All 7 NuGet packages updated to target versions
- [ ] JWT Bearer breaking changes addressed (WebApi only)
- [ ] Solution builds with 0 errors
- [ ] Solution builds with 0 warnings
- [ ] Tests pass (if test projects exist)
- [ ] No merge conflicts
- [ ] Commit message is descriptive

---

### Rollback Procedure

**If issues discovered before merge to main**:
```
# Simply don't merge the branch; stay on upgrade branch for fixes
git reset --hard <last-known-good-commit>
# Or fix issues and commit again
```

**If issues discovered after merge to main**:
```
# Option 1: Revert the merge commit
git revert -m 1 <merge-commit-hash>

# Option 2: Reset branch (only if not shared)
git reset --hard <pre-upgrade-commit>
```

---

### Integration with CI/CD (Future)

When implementing CI/CD:
1. **Pre-merge validation**: Build & test on upgrade branch
2. **Merge gate**: Only merge if all checks pass
3. **Post-merge validation**: Run full test suite on main
4. **Deployment**: Follow deployment strategy after merge

*Note: This plan assumes manual execution; CI/CD integration is optional.*

---

## Success Criteria

### Technical Success Criteria

The upgrade is **complete** when ALL of the following are true:

#### Framework Migration
- ? DavidKuehn.Portfolio.ServiceDefaults targets net10.0
- ? DavidKuehn.Portfolio.WebApi targets net10.0
- ? DavidKuehn.Portfolio.AppHost targets net10.0

#### Package Updates
- ? Aspire.Hosting.AppHost upgraded to 13.1.0
- ? Microsoft.AspNetCore.Authentication.JwtBearer upgraded to 10.0.2
- ? Microsoft.AspNetCore.OpenApi upgraded to 10.0.2
- ? Microsoft.Extensions.Http.Resilience upgraded to 10.2.0
- ? Microsoft.Extensions.ServiceDiscovery upgraded to 10.2.0
- ? OpenTelemetry.Instrumentation.AspNetCore upgraded to 1.15.0
- ? OpenTelemetry.Instrumentation.Http upgraded to 1.15.0
- ? *Optional*: OpenTelemetry.Exporter.OpenTelemetryProtocol upgraded to 1.15.0
- ? *Optional*: OpenTelemetry.Extensions.Hosting upgraded to 1.15.0
- ? *Optional*: OpenTelemetry.Instrumentation.Runtime upgraded to 1.15.0

#### Build Success
- ? `dotnet restore` completes without errors
- ? `dotnet build` completes without errors
- ? `dotnet build` completes without warnings
- ? All project outputs generated (DLLs, EXEs)

#### Code Quality
- ? No source code breaking changes remain
- ? All JWT Bearer API migration issues resolved
- ? No obsolete API usage
- ? No unresolved type references

#### Functional Testing
- ? WebApi authentication can be initialized
- ? AppHost service hosting works correctly
- ? Application starts without runtime errors
- ? Service dependencies resolve correctly

#### Source Control
- ? All changes committed to upgrade branch
- ? Commit message describes all changes
- ? Ready for code review and merge

---

### Validation Checklist

Complete this checklist during execution to confirm success:

| Phase | Validation Point | Status | Notes |
| :--- | :--- | :---: | :--- |
| **Pre-Upgrade** | .NET 10 SDK installed | [ ] | Run: `dotnet --version` |
| **Framework Update** | All 3 projects show net10.0 | [ ] | Check .csproj files |
| **Package Update** | All packages updated in .csproj files | [ ] | Verify version numbers |
| **Restore** | `dotnet restore` succeeds | [ ] | No package resolution errors |
| **Build** | `dotnet build` succeeds | [ ] | 0 errors |
| **Build Quality** | No warnings in build output | [ ] | All projects warn-free |
| **Code Changes** | JWT Bearer imports updated | [ ] | WebApi only |
| **Code Changes** | JWT Bearer method calls updated | [ ] | WebApi only |
| **Functional** | WebApi hosts correctly | [ ] | Manual or test |
| **Functional** | Authentication initializes | [ ] | JWT Bearer setup works |
| **Functional** | AppHost starts | [ ] | Service discovery works |
| **Commit** | Changes committed to branch | [ ] | Single atomic commit |

---

### Definition of Done

**Execution Phase Complete When**:
1. All technical success criteria met ?
2. All validation checklist items checked ?
3. Code review completed (if required) ?
4. Branch ready to merge (or hold for further testing) ?

**Upgrade Complete When**:
1. Changes merged to main branch (or designated target) ?
2. Deployment validated in target environment ?
3. Stakeholders notified of completion ?

---

### Post-Upgrade Verification

After merge and deployment, verify:
- [ ] Application loads in target environment
- [ ] Endpoints respond to requests
- [ ] Authentication works end-to-end
- [ ] No runtime errors in logs
- [ ] Performance acceptable
- [ ] Monitoring shows healthy state

---

### Failure Criteria

The upgrade has **failed** if:
- ? Solution does not build due to unresolved breaking changes
- ? Critical authentication fails at runtime
- ? Service fails to host/start
- ? Package dependency conflicts cannot be resolved
- ? Runtime crashes occur immediately on startup

**Recovery**: Address failure using Rollback Procedure (§9)
