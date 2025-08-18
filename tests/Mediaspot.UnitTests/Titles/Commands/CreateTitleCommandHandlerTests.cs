using Moq;
using Shouldly;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using Mediaspot.Application.Titles.Commands.Create;

namespace Mediaspot.UnitTests.Titles.Commands;

public class CreateTitleCommandHandlerTests
{
    private readonly Mock<ITitleRepository> _mockTitleRepo;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly CreateTitleCommandHandler _handler;

    public CreateTitleCommandHandlerTests()
    {
        _mockTitleRepo = new Mock<ITitleRepository>();
        _mockUow = new Mock<IUnitOfWork>();
        _handler = new CreateTitleCommandHandler(_mockTitleRepo.Object, _mockUow.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Title_When_Name_Is_Unique()
    {
        // Arrange
        var command = new CreateTitleCommand("Unique Name", "Desc", DateTime.UtcNow, TitleType.Movie);

        _mockTitleRepo.Setup(r => r.DoesNameExistAsync(command.Name, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);

        // Act
        var titleId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        titleId.ShouldNotBe(Guid.Empty);

        _mockTitleRepo.Verify(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Name_Is_Not_Unique()
    {
        // Arrange
        var command = new CreateTitleCommand("Duplicate Name", "Desc", DateTime.UtcNow, TitleType.Movie);

        // Le decimos al repositorio falso que el nombre SÍ existe
        _mockTitleRepo.Setup(r => r.DoesNameExistAsync(command.Name, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );

        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}