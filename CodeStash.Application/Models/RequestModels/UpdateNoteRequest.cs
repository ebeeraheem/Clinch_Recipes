using System.ComponentModel.DataAnnotations;

namespace CodeStash.Application.Models.RequestModels;
public class UpdateNoteRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(8000)]
    public string? Content { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    public List<string> Tags { get; set; } = [];
}
