namespace ModularMonolith.Application.Abstractions.Import;

public interface IExcelParser<TRow>
{
    Task<IReadOnlyList<TRow>> ParseAsync(
        Stream stream,
        CancellationToken ct = default);
}