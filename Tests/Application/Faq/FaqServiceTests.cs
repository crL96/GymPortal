using Application.Abstractions.Repositories.Faq;
using Application.Dtos.Faq;
using Application.Services;
using Moq;

namespace Tests.Application.Faq;

public class FaqServiceTests
{
    [Fact]
    public async Task GetFaqsAsync_Should_Return_Faqs_When_Repository_Succeeds()
    {
        // Arrange
        var repoMock = new Mock<IFaqRepository>();

        var faqs = new List<FaqItem>
        {
            new(1, "Title 1", "Content 1"),
            new(2, "Title 2", "Content 2")
        };

        repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(faqs);

        var service = new FaqService(repoMock.Object);

        // Act
        var result = await service.GetFaqsAsync();

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Faqs);
        Assert.Equal(2, result.Faqs!.Count);
    }

    [Fact]
    public async Task GetFaqsAsync_Should_Return_Empty_List_When_No_Faqs()
    {
        // Arrange
        var repoMock = new Mock<IFaqRepository>();

        repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<FaqItem>());

        var service = new FaqService(repoMock.Object);

        // Act
        var result = await service.GetFaqsAsync();

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Faqs);
        Assert.Empty(result.Faqs);
    }

    [Fact]
    public async Task GetFaqsAsync_Should_Return_Failed_When_Repository_Throws_Exception()
    {
        // Arrange
        var repoMock = new Mock<IFaqRepository>();

        repoMock
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new Exception("DB failure"));

        var service = new FaqService(repoMock.Object);

        // Act
        var result = await service.GetFaqsAsync();

        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Faqs);
        Assert.NotNull(result.ErrorMessage);
    }
}
