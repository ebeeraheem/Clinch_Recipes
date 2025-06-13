using System.ComponentModel.DataAnnotations;

namespace CodeStash.Application.Models.RequestModels;
public class CreateNoteRequest
{
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(8000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsPrivate { get; set; } = false;
    public List<string> Tags { get; set; } = [];
}
