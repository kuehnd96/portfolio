# Portfolio Codebase Health Report
**Generated:** 2026-11-04  
**Project:** Portfolio (.NET 10, ASP.NET Core, Clean Architecture, .NET Aspire)

---

## 📊 Overall Health Score: **74/100** (Healthy)

### Verdict
The portfolio project is well-structured with **solid Clean Architecture layering, passing tests (16/16)**, and proper separation of concerns. However, **dependency vulnerabilities present a near-term risk**—five high-severity and four moderate packages need immediate updates. N+1 query patterns in the data layer and dual authentication strategies should be consolidated.

---

## 🎯 Score Breakdown by Category

| Category | Score | Verdict | Status |
|----------|-------|---------|--------|
| **Security** | 54 | ⚠️ Needs work | Multiple auth gaps, 5 HIGH + 4 MODERATE CVEs |
| **Architecture** | 85 | ✅ Strong | Clean layering, proper separation of concerns |
| **Database / ORM** | 62 | ⚠️ Needs work | N+1 queries, manual reader loops |
| **Performance** | 78 | ✅ Healthy | Good async patterns, no allocation issues |
| **Observability** | 76 | ✅ Healthy | OpenTelemetry wired, Aspire dashboard ready |
| **Testing** | 82 | ✅ Healthy | 16/16 passing, real SQL integration tests |
| **Dependencies** | 42 | 🔴 At risk | 5 HIGH + 4 MODERATE NuGet CVEs |
| **Code Quality** | 80 | ✅ Healthy | Zero errors, idiomatic C#, Result pattern |

---

## 📈 Key Signals

| Signal | Value |
|--------|-------|
| **Build Status** | ✅ Passing (0 errors) |
| **Test Results** | ✅ 16 passed, 0 failed |
| **Code Coverage** | ❓ Not measured (recommend adding to CI) |
| **Known Vulnerabilities** | 🔴 5 HIGH (NU1903), 4 MODERATE (NU1902) |
| **Outdated Packages** | ❓ Not measured |
| **Compiler Warnings** | ✅ 0 (123 NuGet vuln warnings = same issues) |

---

## 🔴 Critical Issues (Fix This Week)

### 1. **5 High-Severity NuGet Packages with Known CVEs** [CRITICAL]
**Packages:** Microsoft.OpenApi, System.Security.Cryptography.Pkcs, Microsoft.AspNetCore.Components.WebAssembly.Server, SSH.NET, MessagePack

**Impact:** Production deployment risk; exploitation vectors exist.

**Fix:**
```bash
cd src
dotnet list package --vulnerable
# Update each to latest stable patch
dotnet package update Microsoft.OpenApi
dotnet package update System.Security.Cryptography.Pkcs
# ... etc
dotnet test
```

**Estimate:** 30–60 minutes

---

### 2. **N+1 Query Pattern in GetJob()** [HIGH]
**Location:** `Infrastructure/WorkExperience/Data/WorkExperienceData.cs`  
**Issue:** Fetches Job, then Titles (separate query), then Skills (separate query). Three round-trips per single-job read.

**Current:**
```
SELECT * FROM Job WHERE Id = @id
SELECT * FROM Title WHERE JobId = @jobId
SELECT * FROM Skill WHERE JobId = @jobId
```

**Fix:** Combine into a single batch or use joins:
```
SELECT Job.*, Title.*, Skill.* 
FROM Job 
LEFT JOIN Title ON Title.JobId = Job.Id
LEFT JOIN Skill ON Skill.JobId = Job.Id
WHERE Job.Id = @id
```

**Performance Gain:** ~67% latency reduction per GetJob call.

**Estimate:** 2–3 hours

---

### 3. **Dual Authentication Strategy (API Key + JWT)** [HIGH]
**Location:** WebApi project  
**Issue:** Both X-API-KEY header and JWT Bearer are enabled. No clear hierarchy or mutual exclusion; confuses clients and complicates security model.

**Fix:**
1. Decide: API Key OR JWT (recommend JWT for modern APIs)
2. Remove the unused one entirely
3. Add explicit CORS policy to startup:
```csharp
services.AddCors(options => options
    .AddPolicy("default", p => p
        .WithOrigins("https://yourdomain.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
    ));
```

**Estimate:** 1 hour

---

## 🟠 High-Priority Improvements (This Sprint)

### 4. **Manual SqlDataReader Loops Without Column-Index Caching**
**Location:** `Infrastructure/WorkExperience/Data/WorkExperienceData.cs`

**Issue:** Calling `reader.GetOrdinal("ColumnName")` on every iteration is inefficient.

**Fix:** Cache ordinals upfront:
```csharp
var idOrdinal = reader.GetOrdinal("Id");
while (reader.Read()) {
    var id = reader.GetGuid(idOrdinal);
    // ...
}
```

**Estimate:** 1 hour

---

### 5. **No Central Package Management**
**Issue:** Each `.csproj` pins versions independently. Risk of version drift across the solution.

**Fix:** Create `Directory.Packages.props` at repo root:
```xml
<Project>
  <ItemGroup>
    <PackageReference Update="Microsoft.AspNetCore.OpenApi" Version="10.0.4" />
    <PackageReference Update="FluentValidation" Version="11.9.2" />
    <!-- ... -->
  </ItemGroup>
</Project>
```

Then remove version attributes from all `.csproj` files.

**Estimate:** 2 hours

---

### 6. **No Explicit CORS Configuration**
**Location:** WebApi startup  
**Issue:** Defaults to restrictive, but should be explicit for clarity and security.

**Fix:** Add to WebApi Program.cs:
```csharp
builder.Services.AddCors(options => options
    .AddPolicy("default", p => p
        .WithOrigins("https://your-ui-domain.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
));

app.UseCors("default");
```

**Estimate:** 30 minutes

---

## 🟡 Medium-Priority Improvements (Next Sprint)

### 7. **No Explicit Span Creation in Handlers**
**Location:** UseCases handlers  
**Fix:** Add custom activity spans for domain operations:
```csharp
using var activity = new Activity("GetJob").Start();
activity.AddTag("job.id", jobId);
// ...
```

**Estimate:** 2–3 hours

---

### 8. **Code Coverage Not Measured**
**Fix:** Add to CI pipeline:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
Set target: **75%+ for Core, Application, Infrastructure layers.**

**Estimate:** 1 hour setup

---

## ✅ Strengths

- **Architecture:** Clean layering, strong dependency rule enforcement, clear separation of concerns.
- **Testing:** Comprehensive test suite (unit, handler, integration). Real SQL Server in integration tests (best practice).
- **Code Quality:** Zero compiler errors, nullable reference types enabled, Result pattern used consistently, idiomatic C# with primary constructors.
- **Async:** No sync-over-async anti-patterns, proper CancellationToken usage throughout.
- **Observability:** OpenTelemetry integrated, Aspire dashboard ready, health checks configured.

---

## 🎯 Top 3 Quick Wins (Highest ROI)

1. **Update 5 CVE packages** → Eliminates security risk in 30–60 min
2. **Optimize GetJob() query** → 67% latency gain, 2–3 hours
3. **Clarify auth strategy + add CORS** → Tightens security model, 1–1.5 hours

---

## 📋 Recommended Action Plan

| Priority | Task | Effort | Impact |
|----------|------|--------|--------|
| **P1** | Update high-severity NuGet packages | 1 hour | 🔴 Critical |
| **P1** | Test all updates, regression check | 1 hour | 🔴 Critical |
| **P2** | Optimize N+1 in GetJob() | 2–3 hours | 🟠 High (67% latency) |
| **P2** | Cache SqlDataReader column ordinals | 1 hour | 🟠 High (performance) |
| **P3** | Implement Central Package Management | 2 hours | 🟡 Medium (drift prevention) |
| **P3** | Add explicit CORS policy | 30 min | 🟡 Medium (clarity) |
| **P4** | Add custom observability spans | 2–3 hours | 🟡 Medium (debugging) |
| **P4** | Implement code coverage in CI | 1 hour | 🟡 Medium (quality tracking) |

---

## 📊 Artifacts

- **Interactive Dashboard:** [health-dashboard/index.html](../health-dashboard/index.html)
- **JSON History Record:** [health/health-2026-11-04.json](../health/health-2026-11-04.json)

Re-run this dashboard weekly to track score improvements over time.

---

**Generated by: Codebase Health Dashboard**  
**Next scan recommended:** 2026-11-11 (1 week)
