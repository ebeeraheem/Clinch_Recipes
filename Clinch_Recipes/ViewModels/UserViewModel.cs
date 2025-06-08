using CodeStash.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class UserViewModel
{
}

public class UserProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
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
    public List<string> FavoriteLanguages { get; set; } = [];
    public List<Note> RecentNotes { get; set; } = [];
    public List<Note> PopularNotes { get; set; } = [];
}

public class UserStatsViewModel
{
    public int TotalNotes { get; set; }
    public int PublicNotes { get; set; }
    public int PrivateNotes { get; set; }
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public Dictionary<string, int> LanguageDistribution { get; set; } = new();
    public Dictionary<string, int> NotesPerMonth { get; set; } = new();
    public int NotesThisWeek { get; set; }
    public int NotesThisMonth { get; set; }
    public int NotesThisYear { get; set; }
    public double AverageViewsPerNote { get; set; }
    public string MostUsedLanguage { get; set; } = string.Empty;
    public DateTime FirstNoteDate { get; set; }
    public DateTime LastNoteDate { get; set; }
}

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, hyphens, and underscores")]
    public string Username { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    [Display(Name = "Bio")]
    public string Bio { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(200, ErrorMessage = "Website URL cannot exceed 200 characters")]
    [Display(Name = "Website")]
    public string Website { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "GitHub username cannot exceed 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_-]*$", ErrorMessage = "Invalid GitHub username format")]
    [Display(Name = "GitHub Username")]
    public string GitHubUsername { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Twitter handle cannot exceed 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Invalid Twitter handle format")]
    [Display(Name = "Twitter Handle")]
    public string TwitterHandle { get; set; } = string.Empty;

    [Url(ErrorMessage = "Please enter a valid LinkedIn URL")]
    [StringLength(200, ErrorMessage = "LinkedIn profile URL cannot exceed 200 characters")]
    [Display(Name = "LinkedIn Profile")]
    public string LinkedInProfile { get; set; } = string.Empty;

    [Display(Name = "Make email address public")]
    public bool IsEmailPublic { get; set; }

    [Display(Name = "Favorite Programming Languages")]
    public string FavoriteLanguagesInput { get; set; } = string.Empty;

    public List<string> AvailableLanguages { get; set; } = [];
}

public class UserSettingsViewModel
{
    // Privacy Settings
    [Display(Name = "Make profile public")]
    public bool IsProfilePublic { get; set; } = true;

    [Display(Name = "Show email address")]
    public bool ShowEmail { get; set; } = false;

    [Display(Name = "Show location")]
    public bool ShowLocation { get; set; } = true;

    [Display(Name = "Show social links")]
    public bool ShowSocialLinks { get; set; } = true;

    [Display(Name = "Show statistics")]
    public bool ShowStats { get; set; } = true;

    // Notification Settings
    [Display(Name = "Email notifications for new followers")]
    public bool EmailOnNewFollower { get; set; } = true;

    [Display(Name = "Email notifications for note comments")]
    public bool EmailOnNoteComment { get; set; } = true;

    [Display(Name = "Email notifications for note likes")]
    public bool EmailOnNoteLike { get; set; } = false;

    [Display(Name = "Weekly digest email")]
    public bool WeeklyDigest { get; set; } = true;

    [Display(Name = "Marketing emails")]
    public bool MarketingEmails { get; set; } = false;

    // Display Settings
    [Display(Name = "Theme")]
    public string Theme { get; set; } = "dark";

    [Display(Name = "Notes per page")]
    public int NotesPerPage { get; set; } = 12;

    [Display(Name = "Default note visibility")]
    public string DefaultNoteVisibility { get; set; } = "public";

    [Display(Name = "Code editor font size")]
    public int EditorFontSize { get; set; } = 14;

    [Display(Name = "Show line numbers in code")]
    public bool ShowLineNumbers { get; set; } = true;

    public List<string> AvailableThemes { get; set; } = ["dark", "light", "auto"];
}

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your new password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class PublicUserProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName => !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName)
        ? $"{FirstName} {LastName}".Trim()
        : Username;
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
    public string Email { get; set; } = string.Empty;
    public UserStatsViewModel Stats { get; set; } = new();
    public List<string> FavoriteLanguages { get; set; } = [];
    public List<Note> PublicNotes { get; set; } = [];
    public bool IsCurrentUser { get; set; }
    public bool IsFollowing { get; set; }
}

