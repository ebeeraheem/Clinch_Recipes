namespace CodeStash.Application.Models;

public class PagedResultWithExtras<T, TExtras>
{
    public PagedResult<T> PagedResult { get; set; } = new();
    public required TExtras Extras { get; set; }
}

