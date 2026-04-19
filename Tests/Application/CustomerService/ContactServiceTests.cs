using System;
using Application.Abstractions.Repositories.CustomerService;
using Application.Dtos.CustomerService;
using Application.Services;
using Moq;

namespace Tests.Application.CustomerService;

public class ContactServiceTests
{
    [Fact]
    public async Task SaveContactRequest_Should_Fail_When_Terms_Not_Accepted()
    {
        // Arrange
        var repoMock = new Mock<IContactRequestRepository>();
        var service = new ContactService(repoMock.Object);

        var request = new ContactRequest(
            null,
            "John",
            "Doe",
            "john@test.com",
            null,
            "Hello",
            false,
            DateTime.UtcNow
        );

        // Act
        var result = await service.SaveContactRequest(request);

        // Assert
        Assert.False(result.Succeeded);
        repoMock.Verify(r => r.CreateAsync(It.IsAny<ContactRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SaveContactRequest_Should_Succeed_When_Valid_Request()
    {
        // Arrange
        var repoMock = new Mock<IContactRequestRepository>();

        repoMock
            .Setup(r => r.CreateAsync(It.IsAny<ContactRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContactRequest(
                1,
                "John",
                "Doe",
                "john@test.com",
                null,
                "Hello",
                true,
                DateTime.UtcNow
            ));

        var service = new ContactService(repoMock.Object);

        var request = new ContactRequest(
            null,
            "John",
            "Doe",
            "john@test.com",
            null,
            "Hello",
            true,
            DateTime.UtcNow
        );

        // Act
        var result = await service.SaveContactRequest(request);

        // Assert
        Assert.True(result.Succeeded);
        repoMock.Verify(r => r.CreateAsync(It.IsAny<ContactRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveContactRequest_Should_Fail_When_Repository_Returns_Null()
    {
        // Arrange
        var repoMock = new Mock<IContactRequestRepository>();

        repoMock
            .Setup(r => r.CreateAsync(It.IsAny<ContactRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactRequest?)null);

        var service = new ContactService(repoMock.Object);

        var request = new ContactRequest(
            null,
            "John",
            "Doe",
            "john@test.com",
            null,
            "Hello",
            true,
            DateTime.UtcNow
        );

        // Act
        var result = await service.SaveContactRequest(request);

        // Assert
        Assert.False(result.Succeeded);
    }
}
