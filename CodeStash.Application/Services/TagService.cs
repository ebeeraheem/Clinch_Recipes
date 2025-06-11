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

    public async Task<List<Tag>> GetPopularTagsAsync(int count = 10)
    {
        logger.LogInformation("Fetching top {Count} popular tags", count);

        if (count <= 0)
        {
            logger.LogWarning("Count must be greater than zero. Defaulting to 10.");
            count = 10;
        }

        var popularTags = await context.Tags
            .Include(t => t.Notes)
            .OrderByDescending(t => t.Notes.Count)
            .Take(count)
            .ToListAsync();

        if (popularTags.Count == 0)
        {
            logger.LogWarning("No popular tags found");
        }
        else
        {
            logger.LogInformation("{Count} popular tags found", popularTags.Count);
        }

        return popularTags;
    }
}
