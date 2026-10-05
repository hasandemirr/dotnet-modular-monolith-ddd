namespace ModularMonolith.Application.Abstractions.Excel;

public interface IExcelWorksheet : IDisposable
{
    string? GetString(string address);     // "B1"
    DateTime? GetDateTime(string address); // Excel date
    decimal? GetDecimal(string address);
    bool IsEmpty(string address);

    // Row and column overloads for scanning rows
    string? GetString(int row, int col);
    DateTime? GetDateTime(int row, int col);
    decimal? GetDecimal(int row, int col);
    bool IsEmpty(int row, int col);
}
