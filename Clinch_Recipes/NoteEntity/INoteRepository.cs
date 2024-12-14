namespace Clinch_Recipes.NoteEntity;
public interface INoteRepository
{
    IQueryable<Note> GetAllNotesAsync();
    Task<Note?> GetNoteByIdAsync(Guid id);
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task<int> DeleteNoteAsync(Guid id);
}