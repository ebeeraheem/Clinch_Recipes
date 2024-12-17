using Clinch_Recipes.Models;
using Clinch_Recipes.NoteEntity;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Clinch_Recipes.Controllers;
public class NotesController(INoteRepository noteRepository, IMemoryCache memoryCache) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cacheKey = "notes";
        if (!memoryCache.TryGetValue(cacheKey, out List<NoteViewModel>? notes))
        {
            var query = noteRepository.GetAllNotesAsync();

            notes = await query.Select(note => new NoteViewModel
            {
                Id = note.Id,
                Title = note.Title,
                CreatedDate = note.CreatedDate,
                LastUpdatedDate = note.LastUpdatedDate
            })
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(2048);

            memoryCache.Set(cacheKey, notes, cacheOptions);
        }


        return View(notes);
    }

    public async Task<IActionResult> GetMoreNotes(int page = 2, int pageSize = 20)
    {
        var cacheKey = $"notes_page_{page}";
        if (!memoryCache.TryGetValue(cacheKey, out List<NoteViewModel>? notes))
        {
            var query = noteRepository.GetAllNotesAsync();

            notes = await query
                .Select(x => new NoteViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedDate = x.CreatedDate,
                    LastUpdatedDate = x.LastUpdatedDate
                })
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (notes.Count != 0)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(512);

                memoryCache.Set(cacheKey, notes, cacheOptions);
            }
        }

        return Json(notes);
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

        // Invalidate cache for notes, as new note is added or updated
        var cacheKey = "notes";
        var noteCacheKey = $"note:{note.Id}";

        if (note.Id == Guid.Empty)
        {
            note.CreatedDate = DateTime.UtcNow;
            await noteRepository.AddNoteAsync(note);
            memoryCache.Remove(cacheKey);
            memoryCache.Remove(noteCacheKey);

            return RedirectToAction(nameof(Index));
        }

        await noteRepository.UpdateNoteAsync(note);
        memoryCache.Remove(cacheKey);
        memoryCache.Remove(noteCacheKey);

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

        // Invalidate cache for notes if note is deleted
        var cacheKey = "notes";
        var noteCacheKey = $"note:{id}";

        memoryCache.Remove(cacheKey);
        memoryCache.Remove(noteCacheKey);
        return Json(new { success = true });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var cacheKey = $"note:{id}";

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

        return View(note);
    }
}
