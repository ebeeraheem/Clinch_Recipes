﻿namespace Clinch_Recipes.Entities;
public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Note?> GetNoteByIdAsync(int id);
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(int id);
}