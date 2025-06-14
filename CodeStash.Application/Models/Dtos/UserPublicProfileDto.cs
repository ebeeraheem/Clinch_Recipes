namespace CodeStash.Application.Models.Dtos;
public class UserPublicProfileDto
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
    public List<Note> PublicNotes { get; set; } = [];
    public int PublicNotesTotalCount { get; set; }

    // Public profile settings
    public bool IsEmailPublic { get; set; } = false;
    public bool IsLocationPublic { get; set; } = false;
    public bool IsSocialLinksPublic { get; set; } = true;
}
