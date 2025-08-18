using Moq;
using Shouldly;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using Mediaspot.Application.Titles.Queries;

namespace Mediaspot.UnitTests.Titles.Queries;

public class GetTitleByIdQueryHandlerTests
{
    private readonly Mock<ITitleRepository> _mockRepo;

    public GetTitleByIdQueryHandlerTests()
    {
        _mockRepo = new Mock<ITitleRepository>();
    }

    [Fact]
    public async Task Handle_Should_Return_Title_When_Title_Exists()
    {
        // Arrange
        var titleId = Guid.NewGuid();
        var expectedTitle = new Title("Test Title", "Desc", DateTime.UtcNow, TitleType.Movie);

        _mockRepo.Setup(r => r.GetByIdAsync(titleId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(expectedTitle);

        var handler = new GetTitleByIdQueryHandler(_mockRepo.Object);
        var query = new GetTitleByIdQuery(titleId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(expectedTitle);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Title_Does_Not_Exist()
    {
        // Arrange
        var titleId = Guid.NewGuid();

        _mockRepo.Setup(r => r.GetByIdAsync(titleId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Title?)null);

        var handler = new GetTitleByIdQueryHandler(_mockRepo.Object);
        var query = new GetTitleByIdQuery(titleId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }
}