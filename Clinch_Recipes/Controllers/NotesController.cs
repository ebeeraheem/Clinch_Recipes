﻿using Clinch_Recipes.Entities;
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
            await _noteRepository.AddNoteAsync(note);
            return RedirectToAction(nameof(Index));
        }
        return View(note);
    }

    public async Task<IActionResult> Edit(int id)
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

    public async Task<IActionResult> Delete(int id)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);
        if (note is null)
        {
            return NotFound();
        }
        return View(note);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var isDeleted = await _noteRepository.DeleteNoteAsync(id);
        if (isDeleted)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(await _noteRepository.GetNoteByIdAsync(id));
    }

    public async Task<IActionResult> Details(int id)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);
        if (note is null)
        {
            return NotFound();
        }
        return View(note);
    }
}