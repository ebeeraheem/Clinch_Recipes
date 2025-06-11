using CodeStash.Domain.Entities;

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

public class EditNoteViewModel : CreateNoteViewModel
{
    public string Id { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
