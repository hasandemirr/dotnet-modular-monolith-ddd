using AwesomeAssertions;
using ModularMonolith.Application.Abstractions.Import;

namespace ModularMonolith.Tests.Abstractions.Import;

public sealed class ImportPreviewResultTests
{
    private static ImportPreviewResult<string> Preview(params ImportRowStatus[] statuses) => new()
    {
        Rows = statuses
            .Select((status, index) => new ImportRowResult<string>
            {
                RowNumber = index + 1,
                Status = status,
            })
            .ToList(),
    };

    [Fact]
    public void Counts_MatchRowStatuses()
    {
        var preview = Preview(
            ImportRowStatus.New,
            ImportRowStatus.Unchanged, ImportRowStatus.Unchanged,
            ImportRowStatus.Update, ImportRowStatus.Update, ImportRowStatus.Update,
            ImportRowStatus.Invalid, ImportRowStatus.Invalid, ImportRowStatus.Invalid,
            ImportRowStatus.Invalid,
            ImportRowStatus.Unresolved, ImportRowStatus.Unresolved, ImportRowStatus.Unresolved,
            ImportRowStatus.Unresolved, ImportRowStatus.Unresolved);

        preview.NewCount.Should().Be(1);
        preview.UnchangedCount.Should().Be(2);
        preview.UpdateCount.Should().Be(3);
        preview.InvalidCount.Should().Be(4);
        preview.UnresolvedCount.Should().Be(5);
    }

    [Theory]
    [InlineData(ImportRowStatus.Invalid)]
    [InlineData(ImportRowStatus.Unresolved)]
    public void HasBlockers_WithBlockingRow_IsTrue(ImportRowStatus blockingStatus)
    {
        var preview = Preview(
            ImportRowStatus.New, ImportRowStatus.Unchanged, ImportRowStatus.Update, blockingStatus);

        preview.HasBlockers.Should().BeTrue();
    }

    [Fact]
    public void HasBlockers_WithOnlyNewUnchangedUpdateRows_IsFalse()
    {
        var preview = Preview(
            ImportRowStatus.New, ImportRowStatus.Unchanged, ImportRowStatus.Update);

        preview.HasBlockers.Should().BeFalse();
    }

    [Fact]
    public void HasBlockers_WithNoRows_IsFalse()
    {
        var preview = Preview();

        preview.HasBlockers.Should().BeFalse();
    }
}
