using Clinch_Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.Note;

public class NoteRepository : INoteRepository
{
    private readonly ApplicationDbContext _context;

    public NoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        return await _context.Notes.ToListAsync();
    }

    public async Task<Note?> GetNoteByIdAsync(int id)
    {
        return await _context.Notes.FindAsync(id);
    }

    public async Task AddNoteAsync(Note note)
    {
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNoteAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteNoteAsync(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note is not null)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
