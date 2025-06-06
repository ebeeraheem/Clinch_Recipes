namespace CodeStash.Application.Contracts;
public interface IPagedResultService
{
    Task<PagedResult<T>> GetPagedResultAsync<T>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize)
        where T : class;
}
