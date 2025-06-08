using CodeStash.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class NotesViewModel
{
}

public class MyNotesViewModel
{
    public List<Note> Notes { get; set; } = [];
    public NotesFilterViewModel Filter { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public NotesStatsViewModel Stats { get; set; } = new();
}

public class NotesFilterViewModel
{
    public string SearchQuery { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SortBy { get; set; } = "newest";
    public bool IsPrivateFilter { get; set; } = false;
    public List<string> AvailableLanguages { get; set; } = [];
    public List<Tag> AvailableTags { get; set; } = [];
}

public class PaginationViewModel
{
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 12;
    public int TotalItems { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

public class NotesStatsViewModel
{
    public int TotalNotes { get; set; }
    public int PublicNotes { get; set; }
    public int PrivateNotes { get; set; }
    public int TotalViews { get; set; }
    public int NotesThisMonth { get; set; }
}

public class CreateNoteViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Slug cannot exceed 200 characters")]
    [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Slug can only contain lowercase letters, numbers, and hyphens")]
    [Display(Name = "URL Slug (optional)")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    [StringLength(8000, MinimumLength = 10, ErrorMessage = "Content must be between 10 and 8000 characters")]
    [Display(Name = "Code Content")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Language")]
    public string Language { get; set; } = string.Empty;

    [Display(Name = "Tags (comma-separated)")]
    public string TagsInput { get; set; } = string.Empty;

    [Display(Name = "Make this note private")]
    public bool IsPrivate { get; set; } = false;

    [Display(Name = "Description (optional)")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    public List<string> AvailableLanguages { get; set; } = [];
    public List<Tag> SuggestedTags { get; set; } = [];
}

public class EditNoteViewModel : CreateNoteViewModel
{
    public string Id { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
