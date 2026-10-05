namespace ModularMonolith.Application.Abstractions.Import;

public sealed class ImportPreviewResult<TRow>
{
    public IReadOnlyList<ImportRowResult<TRow>> Rows { get; init; } = [];

    public bool HasBlockers => Rows.Any(r => r.IsBlocker);

    public int NewCount => Rows.Count(r => r.Status == ImportRowStatus.New);
    public int UpdateCount => Rows.Count(r => r.Status == ImportRowStatus.Update);
    public int UnchangedCount => Rows.Count(r => r.Status == ImportRowStatus.Unchanged);
    public int InvalidCount => Rows.Count(r => r.Status == ImportRowStatus.Invalid);
    public int UnresolvedCount => Rows.Count(r => r.Status == ImportRowStatus.Unresolved);
}