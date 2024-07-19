using Clinch_Recipes.NoteEntity;
using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace Clinch_Recipes.Controllers;
public class NotesController : Controller
{
    private readonly INoteRepository _noteRepository;

    public NotesController(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }
    public async Task<IActionResult> Index()
    {
        var notes = await _noteRepository.GetAllNotesAsync();
        return View(notes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Note note)
    {
        if (ModelState.IsValid)
        {
            note.CreatedDate = DateTime.Now;
            note.LastUpdatedDate = DateTime.Now;
            note.Content = Markdown.ToHtml(note.Content);

            await _noteRepository.AddNoteAsync(note);
            return RedirectToAction(nameof(Index));
        }
        return View(note);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);
        if (note is null) return NotFound();

        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Note note)
    {
        if (ModelState.IsValid)
        {
            note.LastUpdatedDate = DateTime.Now;
            await _noteRepository.UpdateNoteAsync(note);
            return RedirectToAction(nameof(Index));
        }
        return View(note);
    }

    public async Task<IActionResult> Upsert(Guid? id)
    {
        var note = new Note();

        if (id is not null)
        {
            // Get the note with the specified id
            note = await _noteRepository.GetNoteByIdAsync((Guid)id);

            return note is null ?
                NotFound() :
                View(note);            
        }

        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Note note)
    {
        if (ModelState.IsValid)
        {
            note.LastUpdatedDate = DateTime.Now;
            note.Content = Markdown.ToHtml(note.Content);

            if (note.Id == Guid.Empty)
            {
                note.CreatedDate = DateTime.Now;                
                await _noteRepository.AddNoteAsync(note);
            }
            else
            {
                await _noteRepository.UpdateNoteAsync(note);
            }

            return RedirectToAction(nameof(Index));
        }

        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await _noteRepository.DeleteNoteAsync(id);
        if (isDeleted)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);
        if (note is null)
        {
            return NotFound();
        }
        return View(note);
    }
}
