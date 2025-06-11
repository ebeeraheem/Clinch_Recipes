namespace CodeStash.Application.Models.QueryParams;

public class MyNotesQueryParams
{
    public string? SearchTerm { get; set; }
    public string SortBy { get; set; } = "newest";
    public bool IsPrivate { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
