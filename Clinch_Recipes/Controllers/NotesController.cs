using Clinch_Recipes.HelperMethods.Pagination;
using Clinch_Recipes.NoteEntity;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Clinch_Recipes.Controllers;
public class NotesController(INoteRepository noteRepository,
                             IMemoryCache memoryCache,
                             IPagedResultService pagedResultService) : Controller
{
    private const int PageSize = 30;

    public async Task<IActionResult> Index()
    {
        const int pageNumber = 1;

        var pagedResult = await GetCachedOrFreshNotesAsync(pageNumber);

        if (pagedResult is null)
        {
            return View(new PagedResult<Note>()
            {
                CurrentPage = pageNumber,
                PageSize = PageSize,
                Items = [],
                TotalCount = 0
            });
        }

        return View(pagedResult);
    }

    private async Task<PagedResult<Note>?> GetCachedOrFreshNotesAsync(int pageNumber)
    {
        var cacheKey = $"notes_page_{pageNumber}";

        // Check if the cache contains the notes
        if (memoryCache.TryGetValue(cacheKey, out PagedResult<Note>? cachedResult))
        {
            return cachedResult;
        }

        // Get the notes from the database
        var query = noteRepository.GetAllNotesAsync();

        var ordered = query.OrderByDescending(n => n.CreatedDate);

        var pagedResult = await pagedResultService
            .GetPagedResultAsync(ordered, pageNumber, PageSize);

        // Cache the notes if they exist
        if (pagedResult.Items.Count != 0)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(2048);

            memoryCache.Set(cacheKey, pagedResult, cacheOptions);
        }

        return pagedResult;
    }

    public async Task<IActionResult> GetNotes(int pageNumber = 2)
    {
        var pagedResult = await GetCachedOrFreshNotesAsync(pageNumber);

        if (pagedResult is null)
        {
            return Json(new { success = false });
        }

        return Json(pagedResult);
    }

    [Authorize]
    public async Task<IActionResult> Upsert(Guid? id)
    {
        var note = new Note();

        if (id is null) return View(note);

        // Get the note with the specified id
        note = await noteRepository.GetNoteByIdAsync((Guid)id);

        return note is null ? NotFound() : View(note);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Upsert(Note note)
    {
        if (!ModelState.IsValid) return View(note);

        note.LastUpdatedDate = DateTime.UtcNow;
        note.Content = Markdown.ToHtml(note.Content);

        // Remove cache for updated note
        var noteCacheKey = $"note_{note.Id}";
        memoryCache.Remove(noteCacheKey);

        // Invalidate cache for all notes
        if (memoryCache is MemoryCache concreteCache)
            concreteCache.Clear();

        if (note.Id == Guid.Empty)
        {
            note.CreatedDate = DateTime.UtcNow;
            await noteRepository.AddNoteAsync(note);

            return RedirectToAction(nameof(Index));
        }

        await noteRepository.UpdateNoteAsync(note);

        return RedirectToAction(nameof(Details), new { id = note.Id });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await noteRepository.DeleteNoteAsync(id);

        if (result <= 0)
        {
            return Json(new { success = false });
        }

        if (memoryCache is MemoryCache concreteCache)
        {
            concreteCache.Clear();
        }

        return Json(new { success = true });
    }

    public IActionResult Details(Guid id)
    {
        return View();
    }

    public async Task<IActionResult> GetNoteFromServer(Guid id)
    {
        var cacheKey = $"note_{id}";

        if (!memoryCache.TryGetValue(cacheKey, out Note? note))
        {
            note = await noteRepository.GetNoteByIdAsync(id);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetPriority(CacheItemPriority.Normal);

            memoryCache.Set(cacheKey, note, cacheOptions);
        }

        if (note is null)
        {
            return NotFound();
        }

        return View(nameof(Details), note);
    }
}
