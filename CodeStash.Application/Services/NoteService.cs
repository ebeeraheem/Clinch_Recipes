using Markdig;
using Slugify;

namespace CodeStash.Application.Services;
public class NoteService(
    ApplicationDbContext context,
    ISlugHelper slugHelper,
    UserHelper userHelper,
    IPagedResultService pagedResultService,
    ILogger<NoteService> logger) : INoteService
{
    public async Task<Result<Note>> CreateNoteAsync(CreateNoteRequest request)
    {
        logger.LogInformation("Creating a new note with title: {Title}", request.Title);

        var userId = userHelper.GetUserId();

        // Validate tags
        var tags = await context.Tags
            .Where(t => request.TagIds.Contains(t.Id))
            .ToListAsync();

        if (tags.Count != request.TagIds.Count)
        {
            logger.LogWarning("Some tags in the request do not exist.");
            return Result<Note>.Failure(NoteErrors.InvalidTags);
        }

        // Generate slug
        var slug = slugHelper.GenerateSlug(request.Title);

        // Check for existing slug
        if (await context.Notes.AnyAsync(n => n.Slug == slug))
        {
            logger.LogWarning("A note with the slug {Slug} already exists.", slug);
            return Result<Note>.Failure(NoteErrors.SlugAlreadyExists);
        }

        // Convert content to Html
        var contentHtml = Markdown.ToHtml(request.Content);

        var note = new Note
        {
            Title = request.Title,
            Content = contentHtml,
            Slug = slug,
            IsPrivate = request.IsPrivate,
            Tags = tags,
            AuthorId = userId,
        };

        await context.Notes.AddAsync(note);
        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to create note with title: {Title}", request.Title);
            return Result<Note>.Failure(NoteErrors.CreateFailed);
        }

        logger.LogInformation("Note created successfully with ID: {NoteId}", note.Id);
        return Result<Note>.Success(note);
    }

    public async Task<Result> UpdateNoteAsync(string noteId, UpdateNoteRequest request)
    {
        logger.LogInformation("Updating note with ID: {NoteId}", noteId);

        var userId = userHelper.GetUserId();

        // Find the note
        var note = await context.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (note is null)
        {
            logger.LogWarning("Note with ID {NoteId} not found.", noteId);
            return Result.Failure(NoteErrors.NotFound);
        }

        if (note.AuthorId != userId)
        {
            logger.LogWarning("Unauthorized access attempt to update note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.ForbiddenAccess);
        }

        // Validate tags
        var tags = await context.Tags
            .Where(t => request.TagIds.Contains(t.Id))
            .ToListAsync();

        if (tags.Count != request.TagIds.Count)
        {
            logger.LogWarning("Some tags in the request do not exist.");
            return Result.Failure(NoteErrors.InvalidTags);
        }

        // Generate slug if title is provided
        var slug = string.IsNullOrWhiteSpace(request.Title) ? note.Slug : slugHelper.GenerateSlug(request.Title);

        // Check for existing slug
        if (await context.Notes.AnyAsync(n => n.Slug == slug && n.Id != noteId))
        {
            logger.LogWarning("A note with the slug {Slug} already exists.", slug);
            return Result.Failure(NoteErrors.SlugAlreadyExists);
        }

        // Update properties
        note.Title = string.IsNullOrWhiteSpace(request.Title) ? note.Title : request.Title;
        note.Content = string.IsNullOrWhiteSpace(request.Content) ? note.Content : Markdown.ToHtml(request.Content);
        note.IsPrivate = request.IsPrivate;
        note.Tags = tags;
        note.Slug = slug;

        context.Notes.Update(note);
        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to update note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.UpdateFailed);
        }

        logger.LogInformation("Note updated successfully with ID: {NoteId}", note.Id);
        return Result.Success();
    }

    public async Task<Result<Note>> GetNoteBySlugAsync(string slug)
    {
        logger.LogInformation("Retrieving note with slug: {Slug}", slug);

        var userId = userHelper.GetUserId();

        var note = await context.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Slug == slug);

        if (note is null)
        {
            logger.LogWarning("Note with slug {Slug} not found.", slug);
            return Result<Note>.Failure(NoteErrors.NotFound);
        }

        //// Check if the note is private and if the user has access
        //if (note.IsPrivate && note.AuthorId != userId)
        //{
        //    logger.LogWarning("Unauthorized access attempt to private note with slug: {Slug}", slug);
        //    return Result<Note>.Failure(NoteErrors.UnauthorizedAccess);
        //}

        // Increment view count
        note.ViewCount++;
        context.Notes.Update(note);
        await context.SaveChangesAsync();

        logger.LogInformation("Note retrieved successfully with ID: {NoteId}", note.Id);
        return Result<Note>.Success(note);
    }

    public async Task<Result<Note>> GetNoteByIdAsync(string noteId)
    {
        logger.LogInformation("Retrieving note with ID: {NoteId}", noteId);

        var userId = userHelper.GetUserId();
        var note = await context.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (note is null)
        {
            logger.LogWarning("Note with ID {NoteId} not found.", noteId);
            return Result<Note>.Failure(NoteErrors.NotFound);
        }

        //// Check if the note is private and if the user has access
        //if (note.IsPrivate && note.AuthorId != userId)
        //{
        //    logger.LogWarning("Unauthorized access attempt to private note with ID: {NoteId}", noteId);
        //    return Result<Note>.Failure(NoteErrors.UnauthorizedAccess);
        //}

        // Increment view count
        note.ViewCount++;
        context.Notes.Update(note);
        await context.SaveChangesAsync();

        logger.LogInformation("Note retrieved successfully with ID: {NoteId}", note.Id);
        return Result<Note>.Success(note);
    }

    public async Task<Result> DeleteNoteAsync(string noteId)
    {
        logger.LogInformation("Deleting note with ID: {NoteId}", noteId);

        var userId = userHelper.GetUserId();
        var note = await context.Notes
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (note is null)
        {
            logger.LogWarning("Note with ID {NoteId} not found or access denied.", noteId);
            return Result.Failure(NoteErrors.NotFound);
        }

        if (note.AuthorId != userId)
        {
            logger.LogWarning("Unauthorized access attempt to delete note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.ForbiddenAccess);
        }

        context.Notes.Remove(note);
        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to delete note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.DeleteFailed);
        }

        logger.LogInformation("Note deleted successfully with ID: {NoteId}", note.Id);
        return Result.Success();
    }

    public async Task<PagedResult<Note>> GetNotesAsync(NoteQueryParams parameters)
    {
        logger.LogInformation("Retrieving notes with parameters: {@Parameters}", parameters);

        var query = context.Notes
            .Include(n => n.Tags)
            .AsQueryable();

        // Filter by title
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            query = query.Where(n => n.Title.Contains(parameters.SearchTerm));
        }

        // Paginate
        return await pagedResultService.GetPagedResultAsync(query, parameters.PageNumber, parameters.PageSize);
    }
}
