using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class UserViewModel
{
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
