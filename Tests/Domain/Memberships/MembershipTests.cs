using Domain.Aggregates.Membership;
using Domain.Aggregates.Membership.Exceptions;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Common.Exceptions;

namespace Tests.Domain.Memberships;

public class MembershipTests
{
    [Fact]
    public void Create_Should_Create_Membership_When_Input_Is_Valid()
    {
        // Arrange
        var name = "Premium";
        var price = 100;

        // Act
        var membership = Membership.Create(name, price);

        // Assert
        Assert.Equal("Premium", membership.Name);
        Assert.Equal(100, membership.Price);
        Assert.NotEqual(Guid.Empty, membership.Id.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var price = 100;

        // Act
        Action act = () => Membership.Create(name, price);

        // Assert
        Assert.Throws<NullOrWhitespaceDomainException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_Should_Throw_When_Price_Is_Invalid(int price)
    {
        // Arrange
        var name = "Premium";

        // Act
        Action act = () => Membership.Create(name, price);

        // Assert
        Assert.Throws<InvalidPriceDomainException>(act);
    }

    [Fact]
    public void Recreate_Should_Keep_Id_And_Set_Values()
    {
        // Arrange
        var id = MembershipId.Create();
        var name = "Basic";
        var price = 50;

        // Act
        var membership = Membership.Recreate(id, name, price);

        // Assert
        Assert.Equal(id, membership.Id);
        Assert.Equal("Basic", membership.Name);
        Assert.Equal(50, membership.Price);
    }

    [Fact]
    public void Update_Should_Change_Name_When_Valid()
    {
        // Arrange
        var membership = Membership.Create("Basic", 50);

        // Act
        membership.Update(name: "Premium");

        // Assert
        Assert.Equal("Premium", membership.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_Should_Throw_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var membership = Membership.Create("Basic", 50);

        // Act
        Action act = () => membership.Update(name: name);

        // Assert
        Assert.Throws<NullOrWhitespaceDomainException>(act);
    }

    [Fact]
    public void Update_Should_Change_Price_When_Valid()
    {
        // Arrange
        var membership = Membership.Create("Basic", 50);

        // Act
        membership.Update(price: 100);

        // Assert
        Assert.Equal(100, membership.Price);
    }

    [Fact]
    public void Update_Should_Ignore_Zero_Or_Negative_Price()
    {
        // Arrange
        var membership = Membership.Create("Basic", 50);

        // Act
        membership.Update(price: 0);

        // Assert
        Assert.Equal(50, membership.Price);
    }
}
