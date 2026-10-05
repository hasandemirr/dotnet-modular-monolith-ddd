namespace ModularMonolith.Application.Abstractions.Excel;

public interface IExcelCellReader
{
    Task<IExcelWorksheet> OpenAsync(Stream stream, int sheetIndex = 1, CancellationToken ct = default);
    Task<IExcelWorksheet> OpenAsync(Stream stream, string sheetName, CancellationToken ct = default);
}
