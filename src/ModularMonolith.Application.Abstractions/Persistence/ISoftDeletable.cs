namespace ModularMonolith.Application.Abstractions.Persistence;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedByUserId { get; }

    void MarkDeleted(DateTime nowUtc, Guid? deletedByUserId);
}