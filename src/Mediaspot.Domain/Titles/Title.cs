namespace Mediaspot.Domain.Titles;

public enum TitleType //TODO: Consider passing to Domain.Common.Enums if it grows or is used in multiple places.
{
    Undefined = 0,
    Movie,
    Series,
    Documentary,
    Other
}

public class Title
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime ReleaseDate { get; private set; }
    public TitleType Type { get; private set; }
    public Title(string name, string description, DateTime releaseDate, TitleType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Title name cannot be empty.", nameof(name));
        }

        if (type == TitleType.Undefined)
        {
            throw new ArgumentException("Title type is undefined, a valid title type must be specified.", nameof(type));
        }

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ReleaseDate = releaseDate;
        Type = type;
    }

    // Private constructor for EF Core
    private Title() { }
}