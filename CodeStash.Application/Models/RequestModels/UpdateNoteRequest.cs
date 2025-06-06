using System.ComponentModel.DataAnnotations;

namespace CodeStash.Application.Models.RequestModels;

public class UpdateNoteRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }
    [MaxLength(8000)]
    public string? Content { get; set; }
    public bool IsPrivate { get; set; } = false;
    public List<string> TagIds { get; set; } = [];
}
