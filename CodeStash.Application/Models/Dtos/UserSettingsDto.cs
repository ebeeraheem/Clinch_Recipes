namespace CodeStash.Application.Models.Dtos;

public class UserSettingsDto
{
    public string Id { get; set; } = string.Empty;

    // Public profile settings
    public bool IsEmailPublic { get; set; }
    public bool IsLocationPublic { get; set; }
    public bool IsSocialLinksPublic { get; set; }
}
