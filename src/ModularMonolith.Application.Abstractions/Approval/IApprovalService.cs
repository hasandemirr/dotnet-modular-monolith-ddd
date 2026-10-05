namespace ModularMonolith.Application.Abstractions.Approval;

public interface IApprovalService
{
    /// <summary>
    /// Creates an approval request for the entity. Throws BusinessRuleViolationException
    /// when no active rule matches the entity type and scope.
    /// </summary>
    Task<Guid> SubmitAsync(
        string entityType,
        Guid entityId,
        string? scope,
        Guid requestedByUserId,
        CancellationToken ct = default);

    Task ApproveAsync(
        Guid approvalRequestId,
        Guid reviewerUserId,
        string? note,
        CancellationToken ct = default);

    Task RejectAsync(
        Guid approvalRequestId,
        Guid reviewerUserId,
        string? note,
        CancellationToken ct = default);

    /// <summary>
    /// Returns whether an active rule exists for the entity type and scope. Callers check
    /// it before submitting.
    /// </summary>
    Task<bool> RequiresApprovalAsync(
        string entityType,
        string? scope,
        CancellationToken ct = default);
}