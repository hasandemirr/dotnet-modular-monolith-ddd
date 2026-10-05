namespace ModularMonolith.Application.Abstractions.Excel;

public interface IExcelTableReader
{
    Task<IReadOnlyList<IDictionary<string, string?>>> ReadSheetAsync(
        Stream stream,
        int sheetIndex = 1,
        bool hasHeader = true, 
        int headerRowIndex = 1,
        CancellationToken ct = default);
}