using CodeStash.Domain.Entities;

namespace CodeStash.ViewModels;

public class HomeViewModel
{
    public List<Note> NewNotes { get; set; } = [];
    public List<Note> PopularNotes { get; set; } = [];
    public SearchFilterViewModel SearchFilter { get; set; } = new();
}

public class SearchFilterViewModel
{
    public string Query { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SortBy { get; set; } = "newest";
    public List<string> AvailableLanguages { get; set; } = [];
    public List<Tag> AvailableTags { get; set; } = [];
}

public class NotesApiResponse
{
    public List<Note> Notes { get; set; } = [];
    public bool HasMore { get; set; }
    public int Page { get; set; }
}
