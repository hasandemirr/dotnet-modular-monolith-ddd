namespace ModularMonolith.Application.Abstractions.Excel;

public interface IExcelTableWriter
{
    Task WriteSheetAsync<T>(
        Stream target,
        string sheetName,
        IReadOnlyList<T> rows,
        CancellationToken ct = default);
}