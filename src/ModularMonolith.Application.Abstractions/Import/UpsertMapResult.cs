namespace ModularMonolith.Application.Abstractions.Import;

public enum UpsertOutcome
{
    Ok = 0,
    Unresolved = 1,
    Invalid = 2,
}

public sealed record UpsertMapResult(UpsertOutcome Outcome, IReadOnlyList<string> Errors)
{
    public static readonly UpsertMapResult Ok = new(UpsertOutcome.Ok, []);
    public static UpsertMapResult Unresolved(params string[] errors) => new(UpsertOutcome.Unresolved, errors);
    public static UpsertMapResult Invalid(params string[] errors) => new(UpsertOutcome.Invalid, errors);
}