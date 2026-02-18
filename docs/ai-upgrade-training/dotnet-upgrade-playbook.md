# Reusable AI Playbook: .NET + NuGet Upgrade

## Objective
Provide a repeatable, low-risk procedure an AI agent can follow to upgrade a .NET solution to a target major version and latest stable packages.

## Inputs
- `target_tfm`: e.g., `net10.0`
- `stability_policy`: `stable-only` or `allow-prerelease`
- `scope`: all projects in solution
- `validation_level`: restore only, build, or test suite

## Deterministic procedure

### Phase 1: Inventory
1. Locate solution files (`*.sln`).
2. Locate all project files (`*.csproj`) included by the solution.
3. Locate shared version files (`Directory.Packages.props`, `global.json`, custom `.props`).
4. Extract all explicit package versions and SDK versions.

### Phase 2: Version discovery
1. For each package, fetch available versions from NuGet metadata.
2. Apply policy filter:
   - If `stable-only`, remove `preview`, `rc`, `beta`, `alpha`.
3. Select highest remaining version.
4. Preserve package-family consistency where possible (same major line for related libraries).

### Phase 3: Upgrade edits
1. Update each project `TargetFramework` to `target_tfm`.
2. Update package versions to selected versions.
3. Update SDK references in project files when present.
4. Keep edits minimal and avoid unrelated formatting changes.

### Phase 4: Verification
1. Run `dotnet restore` on solution.
2. Run `dotnet build -c Debug` on solution.
3. If available and requested, run tests.
4. Report:
   - successes,
   - warnings introduced vs. pre-existing,
   - blocking failures requiring manual decisions.

## Decision log schema (for training data)
For every changed dependency, record:
- `name`
- `old_version`
- `new_version`
- `change_type` (major/minor/patch)
- `why` (1 sentence)
- `risk_mitigation` (how validated)

Example JSON shape:
```json
{
  "name": "Microsoft.AspNetCore.OpenApi",
  "old_version": "9.0.9",
  "new_version": "10.0.3",
  "change_type": "major",
  "why": "Align framework-adjacent package with target .NET major.",
  "risk_mitigation": "Solution restore and build succeeded on net10.0."
}
```

## Quality gates
- All solution projects compile on target TFM.
- No prerelease packages when policy is stable-only.
- No unrelated source-code refactors in same change set.
- Upgrade notes file is generated with rationale and command transcript summary.

## Failure handling
If build fails after version changes:
1. Identify first compile/runtime blockers.
2. Prefer minimal compatibility fix (API rename, package split, using directive).
3. If conflict is ecosystem-level (transitive incompatibility), pin to highest compatible stable and document exception.
4. Do not silently downgrade target TFM without explicit user approval.

## Prompt template for future agents
"Upgrade this .NET solution to `<target_tfm>` and update all NuGet packages using `<stability_policy>`. Keep changes minimal, validate with restore/build, and generate a rationale document containing why each version change was made and how risk was mitigated."
