using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Tests.Domain.TrainingSessions;

public class TrainingSessionIdTests
{
    [Fact]
    public void Create_Should_Return_Valid_TrainingSessionId()
    {
        // Act
        var id = TrainingSessionId.Create();

        // Assert
        Assert.IsType<Guid>(id.Value);
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void Create_Should_Return_Unique_Ids()
    {
        // Act
        var id1 = TrainingSessionId.Create();
        var id2 = TrainingSessionId.Create();

        // Assert
        Assert.NotEqual(id1.Value, id2.Value);
    }

    [Fact]
    public void Recreate_Should_Return_Id_When_Guid_Is_Valid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var id = TrainingSessionId.Recreate(guid);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void Recreate_Should_Throw_When_Guid_Is_Empty()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act & Assert
        Assert.Throws<InvalidIdDomainException>(() =>
            TrainingSessionId.Recreate(guid));
    }
}
