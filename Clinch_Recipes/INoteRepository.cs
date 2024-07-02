using Clinch_Recipes.Entities;

namespace Clinch_Recipes;
public interface INoteRepository
{
    Task AddNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(int id);
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Note?> GetNoteByIdAsync(int id);
    Task UpdateNoteAsync(Note note);
}