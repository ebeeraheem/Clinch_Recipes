using CodeStash.Application.Models.Dtos;
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

        // Get or create tags
        var resolvedTags = new List<Tag>();

        foreach (var tagInput in request.Tags)
        {
            // Try to find existing tag
            var existingTag = await context.Tags
                .FirstOrDefaultAsync(t => t.Name.Equals(tagInput));

            if (existingTag is not null)
            {
                resolvedTags.Add(existingTag);
            }
            else
            {
                // Create new tag if it doesn't exist
                var newTag = new Tag { Name = tagInput.Trim().ToLowerInvariant() };

                await context.Tags.AddAsync(newTag);
                resolvedTags.Add(newTag);
            }
        }

        // Generate unique slug
        string slug;
        try
        {
            slug = await GenerateUniqueSlugAsync(request.Title);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "A unique slug could not be generated for title: {Title}", request.Title);
            return Result<Note>.Failure(NoteErrors.SlugAlreadyExists);
        }

        // Convert content to Html
        var contentHtml = Markdown.ToHtml(request.Content);

        var note = new Note
        {
            Title = request.Title,
            Content = contentHtml,
            Description = request.Description,
            Slug = slug,
            IsPrivate = request.IsPrivate,
            Tags = resolvedTags,
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

        // Get or create tags
        var resolvedTags = new List<Tag>();

        foreach (var tagInput in request.Tags)
        {
            // Try to find existing tag
            var existingTag = await context.Tags
                .FirstOrDefaultAsync(t => t.Name.Equals(tagInput));

            if (existingTag is not null)
            {
                resolvedTags.Add(existingTag);
            }
            else
            {
                // Create new tag if it doesn't exist
                var newTag = new Tag { Name = tagInput.Trim().ToLowerInvariant() };

                await context.Tags.AddAsync(newTag);
                resolvedTags.Add(newTag);
            }
        }

        // Update properties
        note.Title = string.IsNullOrWhiteSpace(request.Title) ? note.Title : request.Title;
        note.Description = string.IsNullOrWhiteSpace(request.Description) ? note.Description : request.Description;
        note.Content = string.IsNullOrWhiteSpace(request.Content) ? note.Content : Markdown.ToHtml(request.Content);
        note.IsPrivate = request.IsPrivate;
        note.Tags = resolvedTags;
        //note.Slug = slug; // Do not update slugs to avoid breaking existing links

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

    public async Task<Result<NoteDetailsDto>> GetNoteBySlugAsync(string slug)
    {
        const int topCount = 3; // Number of related notes to retrieve

        logger.LogInformation("Retrieving note with slug: {Slug}", slug);

        var userId = userHelper.TryGetUserId();

        var noteDetails = await context.Notes
            .AsSplitQuery()
            .Include(n => n.Author)
            .Include(n => n.Tags)
            .Select(n => new NoteDetailsDto
            {
                Note = n,
                AuthorOtherNotes = context.Notes
                    .Where(o => o.AuthorId == n.AuthorId && o.Id != n.Id && !o.IsPrivate)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(topCount)
                    .ToList(),
                RelatedNotes = context.Notes
                    .Where(o => o.Tags.Any(
                        t => n.Tags.Select(nt => nt.Name).Contains(t.Name))
                        && o.Id != n.Id && !o.IsPrivate)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(topCount)
                    .ToList(),
            })
            .FirstOrDefaultAsync(n => n.Note.Slug == slug);

        if (noteDetails is null)
        {
            logger.LogWarning("Note with slug {Slug} not found.", slug);
            return Result<NoteDetailsDto>.Failure(NoteErrors.NotFound);
        }

        // Check if the note is private and if the user has access
        if (noteDetails.Note.IsPrivate && noteDetails.Note.AuthorId != userId)
        {
            logger.LogWarning("Unauthorized access attempt to private note with slug: {Slug}", slug);
            return Result<NoteDetailsDto>.Failure(NoteErrors.UnauthorizedAccess);
        }

        // If related notes are empty, fetch the latest public notes (excluding the current note) as fallback
        if (noteDetails.RelatedNotes.Count == 0)
        {
            noteDetails.RelatedNotes = await context.Notes
                .Where(o => o.Id != noteDetails.Note.Id && !o.IsPrivate)
                .OrderByDescending(o => o.CreatedAt)
                .Take(topCount)
                .ToListAsync();
        }

        // Increment view count
        noteDetails.Note.ViewCount++;
        context.Notes.Update(noteDetails.Note);
        await context.SaveChangesAsync();

        logger.LogInformation("Note retrieved successfully with ID: {NoteId}", noteDetails.Note.Id);
        return Result<NoteDetailsDto>.Success(noteDetails);
    }

    // Get note by ID doesn't increment view count, as it's typically used for editing or management purposes.
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

        // Check if the note is private and if the user has access
        if (note.IsPrivate && note.AuthorId != userId)
        {
            logger.LogWarning("Unauthorized access attempt to private note with ID: {NoteId}", noteId);
            return Result<Note>.Failure(NoteErrors.UnauthorizedAccess);
        }

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

    public async Task<Result> ToggleNotePrivacyAsync(string noteId)
    {
        logger.LogInformation("Toggling privacy for note with ID: {NoteId}", noteId);

        var userId = userHelper.GetUserId();

        var note = await context.Notes.FindAsync(noteId);

        if (note is null)
        {
            logger.LogWarning("Note with ID {NoteId} not found.", noteId);
            return Result.Failure(NoteErrors.NotFound);
        }

        if (note.AuthorId != userId)
        {
            logger.LogWarning("Unauthorized access attempt to toggle privacy for note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.ForbiddenAccess);
        }

        // Toggle the privacy status
        note.IsPrivate = !note.IsPrivate;
        context.Notes.Update(note);
        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to toggle privacy for note with ID: {NoteId}", noteId);
            return Result.Failure(NoteErrors.UpdateFailed);
        }

        logger.LogInformation("Privacy toggled successfully for note with ID: {NoteId}", note.Id);
        return Result.Success();
    }

    public async Task<PagedResult<Note>> GetNotesAsync(NoteQueryParams parameters)
    {
        logger.LogInformation("Retrieving notes with parameters: {@Parameters}", parameters);

        var userId = userHelper.GetUserId();

        var query = context.Notes
            .Where(n => !n.IsPrivate || n.AuthorId == userId) // Include private notes only if the user is the author
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

    public async Task<List<Note>> GetUserPublicNotesAsync(string userName, int pageNumber = 1, int pageSize = 50)
    {
        logger.LogInformation("Retrieving public notes for user: {UserName}, Page: {PageNumber}, Size: {PageSize}",
            userName, pageNumber, pageSize);

        return await context.Notes
            .Include(n => n.Author)
            .Include(n => n.Tags)
            .Where(n => n.Author.UserName == userName && !n.IsPrivate)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<PagedResultWithExtras<Note, UserNotesStatsDto>> GetMyNotesAndStatsAsync(
        MyNotesQueryParams queryParams)
    {
        var userId = userHelper.GetUserId();

        logger.LogInformation("Retrieving notes and stats for user ID: {UserId}. Parameters {@Params}",
            userId, queryParams);

        var notesQuery = context.Notes
            .Where(n => n.AuthorId == userId)
            .Include(n => n.Tags)
            .AsQueryable();

        // Calculate stats before applying filters
        var stats = new UserNotesStatsDto
        {
            TotalNotes = await notesQuery.CountAsync(),
            PublicNotes = await notesQuery.CountAsync(n => !n.IsPrivate),
            PrivateNotes = await notesQuery.CountAsync(n => n.IsPrivate),
            TotalViews = await notesQuery.SumAsync(n => n.ViewCount),
            NotesThisMonth = await notesQuery.CountAsync(
            n => n.CreatedAt.Month == DateTime.UtcNow.Month &&
                n.CreatedAt.Year == DateTime.UtcNow.Year)
        };

        // Filter by privacy
        if (queryParams.IsPrivate)
        {
            notesQuery = notesQuery.Where(n => n.IsPrivate);
        }

        // Filter by search term
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            notesQuery = notesQuery.Where(n => n.Title.Contains(queryParams.SearchTerm));
        }

        // Sort by specified criteria
        notesQuery = queryParams.SortBy switch
        {
            "newest" => notesQuery.OrderByDescending(n => n.CreatedAt),
            "oldest" => notesQuery.OrderBy(n => n.CreatedAt),
            "title" => notesQuery.OrderBy(n => n.Title),
            "views" => notesQuery.OrderByDescending(n => n.ViewCount),
            "modified" => notesQuery.OrderByDescending(n => n.ModifiedAt),
            _ => notesQuery.OrderByDescending(n => n.CreatedAt) // Default to newest
        };

        // Get paginated results
        var pagedNotes = await pagedResultService
            .GetPagedResultAsync(notesQuery, queryParams.PageNumber, queryParams.PageSize);

        logger.LogInformation("Retrieved {Count} notes for user ID: {UserId}", pagedNotes.Items.Count, userId);

        return new PagedResultWithExtras<Note, UserNotesStatsDto>
        {
            PagedResult = pagedNotes,
            Extras = stats
        };
    }

    private async Task<string> GenerateUniqueSlugAsync(string title)
    {
        var baseSlug = slugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        const int suffixLength = 4;
        const int maxAttempts = 10;
        int attempt = 0;

        while (await context.Notes.AnyAsync(n => n.Slug == slug))
        {
            if (++attempt > maxAttempts)
            {
                throw new InvalidOperationException($"Failed to generate a unique slug for '{title}' after {maxAttempts} attempts.");
            }

            // Generate a random suffix
            var suffix = new string(Enumerable.Repeat(chars, suffixLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            slug = $"{baseSlug}-{suffix}";
        }

        return slug;
    }

}
