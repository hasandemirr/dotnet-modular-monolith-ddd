namespace ModularMonolith.Application.Abstractions.Import;

public enum ImportRowStatus
{
    /// <summary>Not in the database; will be created.</summary>
    New = 0,

    /// <summary>In the database with identical content; nothing changes.</summary>
    Unchanged = 1,

    /// <summary>In the database with different content; will be updated.</summary>
    Update = 2,

    /// <summary>Failed validation; must be fixed before confirm.</summary>
    Invalid = 3,

    /// <summary>
    /// A referenced record could not be resolved; must be fixed before confirm.
    /// </summary>
    Unresolved = 4
}