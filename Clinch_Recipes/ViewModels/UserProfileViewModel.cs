using CodeStash.Domain.Entities;

namespace CodeStash.ViewModels;

public class UserProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName => !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName)
        ? $"{FirstName} {LastName}".Trim()
        : UserName;
    public string Bio { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string GitHubUsername { get; set; } = string.Empty;
    public string TwitterHandle { get; set; } = string.Empty;
    public string LinkedInProfile { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public DateTime LastActiveAt { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;
    public bool IsEmailPublic { get; set; }
    public UserStatsViewModel Stats { get; set; } = new();
    //public List<string> FavoriteLanguages { get; set; } = [];
    public List<Note> RecentNotes { get; set; } = [];
    public List<Note> PopularNotes { get; set; } = [];
}
