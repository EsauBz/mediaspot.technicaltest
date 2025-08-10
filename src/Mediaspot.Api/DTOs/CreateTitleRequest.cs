using System.ComponentModel.DataAnnotations;
using Mediaspot.Domain.Titles;

namespace Mediaspot.Api.DTOs;

/// <summary>
/// Request DTO for creating a new title.
/// </summary>
public class CreateTitleRequest
{
    [Required]
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    [Required]
    public TitleType Type { get; set; }
}