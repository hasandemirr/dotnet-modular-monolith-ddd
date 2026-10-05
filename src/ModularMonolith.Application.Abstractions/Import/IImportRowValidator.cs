namespace ModularMonolith.Application.Abstractions.Import;

public sealed record ImportRowValidationResult(
    bool IsValid,
    IReadOnlyList<string> Errors);

public interface IImportRowValidator<TRow>
{
    /// <summary>
    /// Structural and semantic validation of a parsed row. Domain invariants are not
    /// checked here; they run when the entity is created or updated during confirm.
    /// </summary>
    Task<ImportRowValidationResult> ValidateAsync(
        TRow row,
        CancellationToken ct = default);
}