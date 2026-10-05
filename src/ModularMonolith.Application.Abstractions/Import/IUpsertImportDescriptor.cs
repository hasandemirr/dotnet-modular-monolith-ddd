namespace ModularMonolith.Application.Abstractions.Import;

public interface IUpsertImportDescriptor<TRow, TEntity>
    where TEntity : class
{
    string? GetKey(TRow row);

    Task PrepareAsync(IReadOnlyList<TRow> rows, CancellationToken ct = default);

    // Must load tracked entities (not AsNoTracking); Update and Unchanged detection
    // depends on change tracking.
    Task<IReadOnlyDictionary<string, TEntity>> LoadExistingAsync(
        IReadOnlyCollection<string> keys, CancellationToken ct = default);

    UpsertMapResult Create(TRow row, out TEntity? entity);

    UpsertMapResult Apply(TRow row, TEntity existing);
}