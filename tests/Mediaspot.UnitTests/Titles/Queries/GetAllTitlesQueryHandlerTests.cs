using Moq;
using Shouldly;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using Mediaspot.Application.Titles.Queries;

namespace Mediaspot.UnitTests.Titles.Queries;

public class GetAllTitlesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_List_Of_Titles()
    {
        // Arrange
        var mockRepo = new Mock<ITitleRepository>();
        var titleList = new List<Title>
        {
            new ("Title 1", "Desc", DateTime.UtcNow, TitleType.Movie),
            new ("Title 2", "Desc", DateTime.UtcNow, TitleType.Series)
        };

        mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(titleList);

        var handler = new GetAllTitlesQueryHandler(mockRepo.Object);
        var query = new GetAllTitlesQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldBe(titleList);
    }
}