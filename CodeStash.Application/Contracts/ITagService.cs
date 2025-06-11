namespace CodeStash.Application.Contracts;
public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync();
    Task<List<Tag>> GetPopularTagsAsync(int count = 10);
    Task<Result<Tag>> GetTagByIdAsync(string tagId);
}