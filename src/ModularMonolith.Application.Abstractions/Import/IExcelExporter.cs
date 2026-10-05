namespace ModularMonolith.Application.Abstractions.Import;


public interface IExcelExporter<TData>
{
    Task ExportAsync(
        Stream target,
        IReadOnlyList<TData> rows,
        CancellationToken ct = default);
}