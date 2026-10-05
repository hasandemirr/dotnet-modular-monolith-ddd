# CLAUDE.md - dotnet-modular-monolith-ddd

Template for .NET 10 applications built as a modular monolith with Domain-Driven Design
and Clean Architecture. It contains a platform core (authentication, users and roles,
infrastructure adapters, error standard, observability, Excel import/export) and one
example module. It contains no business domain modules.

Read `docs/decisions.md` before proposing any change. Items marked `DECISION PENDING`
are never resolved by you: stop and ask.

## Working rules

1. Everything in this repository is English: code, comments, identifiers, file names,
   commit messages, docs, test names, log messages, error, validation and UI texts.
2. Do only what the prompt asks. Touch no file outside the stated scope. Run no git
   command unless explicitly requested. Make no unrequested refactors.
3. Never claim completion without evidence. After every change run the verification
   commands below and report their actual output.
4. If a symbol, file or behavior cannot be found, say so and stop. Never invent APIs,
   signatures or file contents.
5. Separate verified facts from proposals in every report.
6. Fail closed: missing, broken or ambiguous results are reported as not done.

## Migration rules (delete this section when the migration is complete)

- The current step and its acceptance criteria are in `docs/migration/PLAN.md`.
- The source is a read-only export of the source project, added as an additional
  working directory. Never modify it. Never read `Migrations/` or `SeedData/` there.
- Additive transfer: a file enters this repository only after passing the review
  checklist. The default for every file is "not transferred".
- Copy files taken as-is with shell commands; never re-type their content. Apply
  changes as edits.
- The source namespace `OperationsSuite` becomes `ModularMonolith`.
- Source domain modules (HR, Inventory, Mining, NegvaScholarships) and everything that
  depends on them are not transferred.
- End every step with the transfer report defined in `docs/migration/PLAN.md`.

## Solution topology and dependency rules

| Project | May reference | Must not reference |
|---|---|---|
| `ModularMonolith.Api` (composition root) | all modules, Infrastructure, Application.Abstractions, Contracts | - (holds no business logic) |
| `ModularMonolith.Modules.*` | Application.Abstractions, Contracts, Infrastructure | any other `Modules.*` |
| `ModularMonolith.Application.Abstractions` | BCL only | EF Core, ASP.NET Core, any package |
| `ModularMonolith.Contracts` | nothing | anything |
| `ModularMonolith.Infrastructure` | Application.Abstractions, Contracts | `Modules.*`, Api |

- A module that needs something from another module depends on a contract in
  Application.Abstractions, never on the other module.
- Cross-module read: a directory interface in `Application.Abstractions/Directories/`,
  implemented by the owning module. Never access another module's entities.
- Cross-module notification: a callback interface in Application.Abstractions.
  Callback implementations never call `SaveChangesAsync`; the orchestrator commits once.

## Module anatomy

```
ModularMonolith.Modules.<Module>/
  Application/<Context>/Abstractions/      service interfaces
  Application/<Context>/Validators/
  Domain/<Context>/                        entities, value types, enums
  Infrastructure/Persistence/Configs/      IEntityTypeConfiguration<T>
  Infrastructure/Services/<Context>/       service, query service, directory, callback
  <Module>Module.cs                        Add<Module>Module DI registration
```

- Group by context, never by pattern: no `Callbacks/`, `Handlers/` or `Directories/`
  folders inside a module.
- Namespaces mirror folders exactly. File name equals type name.

## Domain rules

- No public setters: `private set` or `init`.
- Private parameterless constructor for EF. Entities are created only through named
  factory methods (`Create`, `Issue`, ...).
- Behavior lives on the entity: `entity.ChangeX(...)`, never `entity.X = ...` from a
  service.
- Child entities have an `internal` factory and are created only through the
  aggregate root.
- Guard clauses enforce invariants. Never throw `InvalidOperationException`. Guard
  exception type: DECISION PENDING (P-11).
- Audit fields (`CreatedAtUtc`, `CreatedByUserId`, `UpdatedAtUtc`, `UpdatedByUserId`)
  are never set in domain code; `AppDbContext` stamps them on save.
- Aggregates with concurrent writes carry `byte[] RowVersion`; child entities do not.
  Aggregate boundary equals concurrency boundary.

## Persistence

- EF Fluent API only, in `Infrastructure/Persistence/Configs/`. No data annotations.
- One schema per module. No cross-schema foreign keys.
- LINQ method syntax only.
- One commit per operation. Callbacks and domain code never call `SaveChangesAsync`.
- Soft delete through `ISoftDeletable` and a global query filter.
- One `AppDbContext`, one migration history, in Infrastructure.

## Application and API

- Services: `<Aggregate>Service` for commands, `<Aggregate>QueryService` for reads.
- DTOs live in Contracts, are `sealed record`, and are named
  `{Aggregate}{Action}{Suffix}`. List queries are positional records bound with
  `[AsParameters]`. Paging uses `PagedResult<T>`.
- Validation in two layers: FluentValidation at the boundary, guard clauses in the
  domain.
- Every Minimal API endpoint has:
  - `MapGroup("/api/v1/...")` and `WithTags(...)`;
  - a permission policy covering it (group or endpoint level);
  - `WithSummary(...)` and `WithDescription(...)`;
  - `Produces<T>` with a Contracts DTO (no anonymous types); creates return
    `IdResponse` with 201;
  - `ProducesProblem` for every plausible error status;
  - no DTO declared in the endpoint file.

| Verb | Success | Errors |
|---|---|---|
| POST (create) | 201 | 400, 401, 403, 409 |
| POST (action) | 200 / 204 | 400, 401, 403 |
| GET (single) | 200 | 401, 404 |
| GET (list) | 200 | 401 |
| PUT / PATCH | 204 | 400, 401, 403, 404, 409 |
| DELETE | 204 | 401, 403, 404 |

## Errors

| Exception | Status |
|---|---|
| `EntityNotFoundException` | 404 |
| `BusinessRuleViolationException` | 422 |
| `ConflictException` | 409 |
| `UnauthorizedDomainException` | 403 |
| `AuthenticationException` | 401 |
| `DbUpdateConcurrencyException` | 409, `code: "concurrency_conflict"` |
| FluentValidation `ValidationException` | 400 |
| anything else | 500 |

- Every error response is RFC 7807 ProblemDetails and carries `correlationId`.
- A 500 response never exposes exception details; details go to the log only.

## Configuration

- Strongly typed options: `public const string SectionName`, `init` properties,
  consumed through `IOptions<T>`.
- No secrets in `appsettings.json`. Secrets come from user secrets (development) or
  environment variables (production, `Section__Key`).

## Testability

- No `DateTime.UtcNow` / `DateTime.Now`: use `IClock`.
- No `Guid.NewGuid()`: use `IGuidGenerator`.
- No `HttpContext` in services: use `ICurrentUser`.
- External I/O stays behind interfaces.
- Allowed exceptions: the adapters themselves (`SystemClock`, `GuidGenerator`) and
  startup code outside the DI chain. Any other exception is recorded in
  `docs/decisions.md`.
- Domain logic gets unit tests using `TestClock` and `TestGuidGenerator`.

## Authorization

- Permission constants follow `{module}.{resource}.{action}` and live in nested classes
  of `Permissions`. One policy per permission. Endpoints request capabilities with
  `RequireAuthorization(Permissions.X)`.
- No role-string checks in code. Decisions use permission claims. The admin role
  receives the full permission catalog from code.

## Import and export

- Two-phase import: preview writes nothing; confirm re-evaluates, applies in a single
  transaction and is idempotent.
- Per module: an Excel parser, a row validator and an upsert descriptor; the generic
  upsert pipeline orchestrates.
- Every row problem is reported as a row status with messages. A bad row never turns
  into a 500.
- Export uses exactly the import columns: exporting and re-importing unchanged data
  yields only `Unchanged` rows.

## Review checklist

Apply to every file before it enters the repository and to every change.

1. Dependency rules are respected.
2. Domain rules are respected.
3. Exceptions follow the error table; no `InvalidOperationException`.
4. No `DateTime.UtcNow`, `Guid.NewGuid()` or `HttpContext` outside the allowed places.
5. Options pattern is used; no secrets in configuration files.
6. Endpoint standard is met.
7. No role-string checks.
8. No dead code: unused types, commented-out code, debug statements, unused
   configuration keys, stray files.
9. English only; no non-English text anywhere.
10. No company traces (trace list in `docs/migration/PLAN.md`).
11. Folder and naming rules: context grouping, no pattern folders, namespace equals
    folder, file name equals type name.

## Commands

Run from the repository root.

```
dotnet build
dotnet test
dotnet ef migrations add <Name> --project src/ModularMonolith.Infrastructure --startup-project src/ModularMonolith.Api
```

## Verification after every change

Report the actual output of:

```
dotnet build
dotnet test
git status
git grep -n --untracked -E "DateTime\.(UtcNow|Now)|Guid\.NewGuid\(\)|InvalidOperationException|RequireRole\(|Role is " -- src
git grep -n --untracked -P "[^\x00-\x7F]" -- src tests
```

The last command flags any non-ASCII character in code, including decorative
characters in comments; replace them with ASCII.

## Commit standard

```
<type>(<scope>): <Subject in imperative mood>
- <Technical detail>
- <Technical detail>
Branch: main
```

- type: feat | fix | refactor | chore | docs | perf | test
- scope: abstractions, contracts, infrastructure, api, users, auth, tests, build, ci,
  docs, plus the example module and Approval (if kept). Sub-scopes such as
  `infrastructure/excel`, `api/middleware`. Multiple scopes are comma-separated.
- subject: imperative, first letter capitalized, no trailing period, at most 70
  characters.
- body: bullets starting with `- `; precise technical detail, what and why.
- footer: always `Branch: main`.
