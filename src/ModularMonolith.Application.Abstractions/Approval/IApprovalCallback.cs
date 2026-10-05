namespace ModularMonolith.Application.Abstractions.Approval;

/// <summary>
/// Notifies the module that owns an approvable entity when its approval request is
/// approved or rejected. Each module implements it for its own entity type key.
/// Implementations never call SaveChangesAsync; the orchestrator commits once.
/// </summary>
public interface IApprovalCallback
{
    /// <summary>
    /// Stable key of the approvable entity type in the form "{module}.{entity}", for
    /// example "catalog.item". Approval rules and requests store this key; a key in use
    /// is never renamed.
    /// </summary>
    string EntityType { get; }

    Task OnApprovedAsync(Guid entityId, CancellationToken ct = default);
    Task OnRejectedAsync(Guid entityId, CancellationToken ct = default);
}