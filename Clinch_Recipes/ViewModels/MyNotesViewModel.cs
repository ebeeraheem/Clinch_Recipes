using CodeStash.Application.Models;
using CodeStash.Application.Models.Dtos;
using CodeStash.Application.Models.QueryParams;
using CodeStash.Domain.Entities;

namespace CodeStash.ViewModels;

public class MyNotesViewModel
{
    public PagedResult<Note> PagedResult { get; set; } = new();
    public UserNotesStatsDto Stats { get; set; } = new();
    public MyNotesQueryParams Filter { get; set; } = new();
}

public class EditNoteViewModel : CreateNoteViewModel
{
    public string Id { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
