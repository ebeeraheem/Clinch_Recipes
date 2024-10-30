using Clinch_Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.NoteEntity;

public class NoteRepository(ApplicationDbContext context) : INoteRepository
{
    public IQueryable<Note> GetAllNotesAsync()
    {
        return context.Notes.AsQueryable();
    }

    public async Task<Note?> GetNoteByIdAsync(Guid id)
    {
        return await context.Notes.FindAsync(id);
    }

    public async Task AddNoteAsync(Note note)
    {
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNoteAsync(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteNoteAsync(Guid id)
    {
        var note = await context.Notes.FindAsync(id);
        if (note is null) return false;
        
        context.Notes.Remove(note);
        await context.SaveChangesAsync();

        return true;
    }
}
