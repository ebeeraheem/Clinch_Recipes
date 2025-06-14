namespace CodeStash.Application.Models.Dtos;
public class NoteDetailsDto
{
    public Note Note { get; set; } = new();
    public List<Note> AuthorOtherNotes { get; set; } = [];
    public List<Note> RelatedNotes { get; set; } = [];
}
