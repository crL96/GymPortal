using System;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.UserMemberships;
using Domain.Common.Exceptions;

namespace Tests.Domain.UserMemberships;

public class UserMembershipTests
{
    [Fact]
    public void Create_Should_Create_Active_UserMembership_When_Input_Is_Valid()
    {
        // Arrange
        var userId = "user-1";
        var membershipId = MembershipId.Create();

        // Act
        var userMembership = UserMembership.Create(userId, membershipId);

        // Assert
        Assert.Equal(userId, userMembership.UserId);
        Assert.Equal(membershipId, userMembership.MembershipId);
        Assert.True(userMembership.IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_UserId_Is_Invalid(string userId)
    {
        // Arrange
        var membershipId = MembershipId.Create();

        // Act
        Action act = () => UserMembership.Create(userId, membershipId);

        // Assert
        Assert.Throws<InvalidIdDomainException>(act);
    }

    [Fact]
    public void Recreate_Should_Create_UserMembership_With_Correct_State()
    {
        // Arrange
        var userId = "user-1";
        var membershipId = MembershipId.Create();
        var isActive = false;

        // Act
        var userMembership = UserMembership.Recreate(userId, membershipId, isActive);

        // Assert
        Assert.Equal(userId, userMembership.UserId);
        Assert.Equal(membershipId, userMembership.MembershipId);
        Assert.False(userMembership.IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Recreate_Should_Throw_When_UserId_Is_Invalid(string userId)
    {
        // Arrange
        var membershipId = MembershipId.Create();

        // Act
        Action act = () => UserMembership.Recreate(userId, membershipId, true);

        // Assert
        Assert.Throws<InvalidIdDomainException>(act);
    }

    [Fact]
    public void ChangePlan_Should_Update_MembershipId()
    {
        // Arrange
        var userMembership = UserMembership.Create("user-1", MembershipId.Create());
        var newMembershipId = MembershipId.Create();

        // Act
        userMembership.ChangePlan(newMembershipId);

        // Assert
        Assert.Equal(newMembershipId, userMembership.MembershipId);
    }

    [Fact]
    public void CancelPlan_Should_Set_IsActive_To_False()
    {
        // Arrange
        var userMembership = UserMembership.Create("user-1", MembershipId.Create());

        // Act
        userMembership.CancelPlan();

        // Assert
        Assert.False(userMembership.IsActive);
    }

    [Fact]
    public void ReactivatePlan_Should_Set_IsActive_To_True()
    {
        // Arrange
        var userMembership = UserMembership.Create("user-1", MembershipId.Create());
        userMembership.CancelPlan();

        // Act
        userMembership.ReactivatePlan();

        // Assert
        Assert.True(userMembership.IsActive);
    }
}
