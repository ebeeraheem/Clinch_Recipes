namespace CodeStash.Application.Services;
public class TagService(ApplicationDbContext context, ILogger<TagService> logger) : ITagService
{
    public async Task<Result<Tag>> GetTagByIdAsync(string tagId)
    {
        logger.LogInformation("Fetching tag with ID: {TagId}", tagId);

        var tag = await context.Tags.FindAsync(tagId);

        if (tag is null)
        {
            logger.LogWarning("Tag with ID {TagId} not found", tagId);
            return Result<Tag>.Failure(TagErrors.NotFound);
        }

        logger.LogInformation("Tag with ID {TagId} found", tagId);
        return Result<Tag>.Success(tag);
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        logger.LogInformation("Fetching all tags");

        var tags = await context.Tags.ToListAsync();

        if (tags.Count == 0)
        {
            logger.LogWarning("No tags found");
        }
        else
        {
            logger.LogInformation("{Count} tags found", tags.Count);
        }

        return tags;
    }
}
