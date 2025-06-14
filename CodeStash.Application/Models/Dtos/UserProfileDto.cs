namespace CodeStash.Application.Models.Dtos;
public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Bio { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; } = string.Empty;
    public string? GitHubUsername { get; set; } = string.Empty;
    public string? TwitterHandle { get; set; } = string.Empty;
    public string? LinkedInProfile { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public List<Note> RecentNotes { get; set; } = [];
    public List<Note> PopularNotes { get; set; } = [];

    // Stats
    public int TotalNotes { get; set; }
    public double AverageViewsPerNote { get; set; }
    public int TotalViews { get; set; }
    public int NotesThisWeek { get; set; }
    public int NotesThisMonth { get; set; }
    public int NotesThisYear { get; set; }
    public Dictionary<string, int> NotesPerMonth { get; set; } = [];
}
