namespace CodeStash.Application.Models.Dtos;
public class UserNotesStatsDto
{
    public int TotalNotes { get; set; }
    public int PublicNotes { get; set; }
    public int PrivateNotes { get; set; }
    public int TotalViews { get; set; }
    public int NotesThisMonth { get; set; }
}
