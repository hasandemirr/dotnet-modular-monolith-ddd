# Decision Log

Single source of decisions for this repository. A recorded decision is not re-debated.
Only new evidence can supersede it; the superseding decision is added as a new entry
that references the old one, and the old entry's status is updated.

Pending items (`P-xx`) are resolved only by the repository owner. Until then they are
marked `DECISION PENDING` wherever they block work.

Status values: `Accepted` | `Superseded by D-xxx`

---

## Accepted

### D-001 - Fresh repository, no inherited history
- Date: 2026-10-02
- Status: Accepted
- Decision: The template lives in a new repository with its own history. No commits are
  carried over from the source project.
- Rationale: The source history contains company-specific data (internal addresses,
  personal e-mail, environment details) that must not become public.

### D-002 - Migrations are regenerated
- Date: 2026-10-02
- Status: Accepted
- Decision: No migration is transferred. After domain removal a single `InitialCreate`
  containing only platform tables is generated. The template ships with it so that
  clone -> database update -> run works.
- Rationale: The source `InitialCreate` contains domain tables. Startup seeding
  (roles, admin user) requires the platform schema to exist.

### D-003 - Additive transfer
- Date: 2026-10-05
- Status: Accepted
- Decision: Start from an empty solution and transfer code layer by layer in dependency
  order. Every file passes the review checklist in `CLAUDE.md` before it enters the
  repository. The default for any file is "not transferred".
- Rationale: Copy-then-delete keeps unreviewed content by default (stray files,
  commented-out code, dead types were found in the source).

### D-004 - Execution by Claude Code, review in the chat project
- Date: 2026-10-05
- Status: Accepted
- Decision: Claude Code performs the transfer in this repository. A separate Claude chat
  project is used for design, Claude Code prompt generation and independent review
  (via repomix). The owner commits and pushes.

### D-005 - Source access via a read-only export
- Date: 2026-10-05
- Status: Accepted
- Decision: Claude Code reads a `git archive` export of the source repository directly
  (not a repomix file). `Migrations/` and every `SeedData/` folder are deleted from the
  export. The export is never modified. Files taken as-is are copied with shell
  commands; changes are applied as edits.
- Rationale: On-demand reads and shell copies cost far fewer tokens than re-typing file
  content, preserve line endings and encoding, and give a complete view (the repomix
  snapshot excludes several folders). `SeedData` may contain personal data.
- Precondition: All VM-only fixes are committed to the source repository before export.

### D-006 - Domain modules are not transferred
- Date: 2026-10-05
- Status: Accepted
- Decision: The source project's business domain modules, and everything that depends
  on them, stay out of the template.
- Note: One of them, a scholarship module, is incomplete. It will be redesigned from
  its data in the first derived project, not ported.

### D-007 - One example module
- Date: 2026-10-05
- Status: Accepted
- Decision: The template contains one example module that demonstrates every rule in
  `CLAUDE.md` with working code. Its tables are created by a separate migration so
  derived projects can remove it cleanly.
- Open: domain and name of the module (P-02).

### D-008 - Naming
- Date: 2026-10-05
- Status: Accepted
- Decision: Repository and chat project: `dotnet-modular-monolith-ddd`.
  Root namespace: `ModularMonolith` (also the `dotnet new` sourceName).
- Rationale: The sourceName is replaced everywhere it appears, so it must be distinctive.
  The word "Template" stays out of the namespace because it appears throughout the docs.

### D-009 - English only
- Date: 2026-10-05
- Status: Accepted
- Decision: Everything in the repository is English: code, comments, identifiers, file
  names, commit messages, documentation, test names, log messages, error, validation
  and UI texts. Turkish strings found in transferred code are translated.
- Supersedes: source README section 10 (user-facing texts in Turkish).
- Note: Localization is a decision of each derived project.

### D-010 - Commit standard
- Date: 2026-10-05
- Status: Accepted
- Decision: Conventional Commits in English.
  - Header: `<type>(<scope>): <Subject>`; imperative mood, first letter capitalized, no
    trailing period, at most 70 characters.
  - Types: feat, fix, refactor, chore, docs, perf, test.
  - Body: bullets starting with `- `, precise technical detail (what and why).
  - Footer: always `Branch: main`. Single branch, no feature branches.

### D-011 - Visibility
- Date: 2026-10-05
- Status: Accepted
- Decision: The repository stays private until the Step 12 trace scan passes and the
  history is squashed into a single commit. It is then made public and marked as a
  GitHub template repository. Derived projects are private.

### D-012 - Excel import/export stays in the template
- Date: 2026-10-05
- Status: Accepted
- Decision: The Excel infrastructure (readers, writer, exporter abstraction, upsert
  import pipeline, import lock) is part of the platform core.
- Rationale: The first derived project needs a one-time legacy import, ongoing
  import/export and a bank transfer file export.

### D-013 - Separate chat project
- Date: 2026-10-05
- Status: Accepted
- Decision: The template has its own Claude chat project. The source project's chat
  project remains dedicated to the source repository.
- Rationale: Mixing snapshots would make reviews verify against the wrong code.

### D-014 - Scoped source snapshot for the chat project
- Date: 2026-10-05
- Status: Accepted
- Decision: The chat project receives a repomix of the source's platform parts only
  (`repomix-source-reference.xml`). It is removed when the transfer is complete.

### D-015 - Approval module is kept
- Date: 2026-10-05
- Status: Accepted
- Decision: The Approval module is part of the template as
  `ModularMonolith.Modules.Approval`. The `ApprovableEntityType` enum is replaced by a
  string key so that adding a module never edits a shared enum (Steps 1 and 6).
- Resolves: P-01.

### D-016 - Central package management
- Date: 2026-10-05
- Status: Accepted
- Decision: Package versions live only in `Directory.Packages.props`. `TargetFramework`,
  `Nullable` and `ImplicitUsings` live in `Directory.Build.props`. A `.csproj` never
  contains a package version. A package is added in the step that transfers the code
  using it, with the source version unless a decision says otherwise.
- Resolves: P-04. The rule is added to README in Step 11.

### D-017 - Folder layout
- Date: 2026-10-05
- Status: Accepted
- Decision: Platform projects in `src/`, modules in `src/Modules/`, test projects in
  `tests/`. The repository root holds solution-level files only.
- Resolves: P-05.

### D-018 - Assertion library
- Date: 2026-10-05
- Status: Accepted
- Decision: AwesomeAssertions (Apache-2.0) replaces FluentAssertions.
- Rationale: The source tests use FluentAssertions 7.2.0 (Apache-2.0); version 8 and
  later are under commercial terms, so a routine upgrade in a derived project would
  change the license silently. AwesomeAssertions is the maintained community fork that
  keeps Apache-2.0.
- Consequence: Transferred tests change the `using` directive and may need API
  adjustments (Step 8).
- Resolves: P-10.

### D-019 - Target framework .NET 10
- Date: 2026-10-05
- Status: Accepted
- Decision: All projects target `net10.0`. `global.json` pins SDK 10.0.100 with
  `rollForward: latestFeature`. Packages versioned with the runtime (EF Core, ASP.NET
  Core, Microsoft.Extensions) take the latest 10.0.x patch in the step that transfers
  the code using them; this overrides the source version rule of D-016 for those
  packages. Docker base images move to 10.0 in Step 11.
- Rationale: The source targets .NET 9, which reaches end of support on 2026-11-10.
  .NET 10 is an LTS release supported until November 2028.
- Consequence: Transferred code may hit .NET 10 breaking changes. Each step fixes them
  under its build and tests and lists them in the transfer report.
- Resolves: P-12 (raised in Step 0).

---

## Pending

| ID | Question | Recommendation | Blocks |
|---|---|---|---|
| P-02 | Domain and name of the example module | Neutral and import-friendly, e.g. a catalog item with a unique code (import key) and a child collection | Step 10 |
| P-03 | Auth model: magic link only? Behavior when `AllowedDomain` is empty? | Magic link only; empty `AllowedDomain` disables the domain restriction | Step 5 |
| P-06 | Source README section 11 (access scope standard) is not implemented in the source code | Move to an optional pattern document, not normative | Step 11 |
| P-07 | `dotnet new` packaging (template.json, sourceName) and a module item template | Do it before the first derived project | After Step 12 |
| P-08 | Frontend (React) template: separate repository or monorepo | Decide before frontend work starts | - |
| P-09 | License and permission to publish code derived from the source project | Resolve before going public | Step 12 |
| P-11 | Exception type for guard clauses | `ArgumentException` only for programmer errors (e.g. empty ids passed by code); business invariants reachable from user input throw domain exceptions | Steps 3-6 |
