using Clinch_Recipes.Models;
using Clinch_Recipes.NoteEntity;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.Controllers;
public class NotesController(INoteRepository noteRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var query = noteRepository.GetAllNotesAsync();
        var notes = await query.Select(x => new NoteViewModel
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                LastUpdatedDate = x.LastUpdatedDate
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
        
        return View(notes);
    }
    
    public async Task<IActionResult> Upsert(Guid? id)
    {
        var note = new Note();

        if (id is null) return View(note);
        
        // Get the note with the specified id
        note = await noteRepository.GetNoteByIdAsync((Guid)id);

        return note is null ?
            NotFound() :
            View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Note note)
    {
        if (!ModelState.IsValid) return View(note);
        
        note.LastUpdatedDate = DateTime.UtcNow;
        note.Content = Markdown.ToHtml(note.Content);

        if (note.Id == Guid.Empty)
        {
            note.CreatedDate = DateTime.UtcNow;                
            await noteRepository.AddNoteAsync(note);
        }
        else
        {
            await noteRepository.UpdateNoteAsync(note);
        }

        return RedirectToAction(nameof(Index));

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await noteRepository.DeleteNoteAsync(id);
        
        return Json(isDeleted ? new { success = true } : new { success = false });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var note = await noteRepository.GetNoteByIdAsync(id);
        if (note is null)
        {
            return NotFound();
        }
        return View(note);
    }
}
