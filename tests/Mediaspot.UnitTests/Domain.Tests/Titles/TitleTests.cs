using Mediaspot.Domain.Titles;
using Shouldly;

namespace Mediaspot.UnitTests.Domain.Tests.Titles;

public class TitleTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var name = "Pulp Fiction";
        var description = "The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption.";
        var releaseDate = new DateTime(1994, 10, 14);
        var type = TitleType.Movie;

        // Act
        var title = new Title(name, description, releaseDate, type);

        // Assert
        title.Id.ShouldNotBe(Guid.Empty);
        title.Name.ShouldBe(name);
        title.Description.ShouldBe(description);
        title.ReleaseDate.ShouldBe(releaseDate);
        title.Type.ShouldBe(type);
    }

    [Fact]
    public void Constructor_Should_Throw_ArgumentException_If_Name_Is_Empty()
    {
        // Arrange
        var emptyName = "";

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new Title(emptyName, "desc", DateTime.UtcNow, TitleType.Movie)
        );
    }

    [Fact]
    public void Constructor_Should_Throw_ArgumentException_If_Type_Is_Undefined()
    {
        // Arrange
        var undefinedType = TitleType.Undefined;

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new Title("A Good Name", "desc", DateTime.UtcNow, undefinedType)
        );
    }
}