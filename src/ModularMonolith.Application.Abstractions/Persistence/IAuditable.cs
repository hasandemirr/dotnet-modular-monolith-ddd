namespace ModularMonolith.Application.Abstractions.Persistence;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    Guid? CreatedByUserId { get; }
    DateTime UpdatedAtUtc { get; }
    Guid? UpdatedByUserId { get; }
}