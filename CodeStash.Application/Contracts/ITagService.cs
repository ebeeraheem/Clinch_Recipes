namespace CodeStash.Application.Contracts;
public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync();
    Task<Result<Tag>> GetTagByIdAsync(string tagId);
}