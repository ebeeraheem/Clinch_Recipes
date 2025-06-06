namespace CodeStash.Application.Models.QueryParams;
public class NoteQueryParams
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
