namespace ModularMonolith.Application.Abstractions.Import;

public sealed class ImportRowResult<TRow>
{
    public int RowNumber { get; init; }
    public ImportRowStatus Status { get; init; }
    public TRow? Data { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];

    public bool IsBlocker =>
        Status is ImportRowStatus.Invalid or ImportRowStatus.Unresolved;
}