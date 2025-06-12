using CodeStash.Application.Models.Dtos;
using CodeStash.Application.Models.QueryParams;

namespace CodeStash.Application.Contracts;
public interface INoteService
{
    Task<Result<Note>> CreateNoteAsync(CreateNoteRequest request);
    Task<Result> DeleteNoteAsync(string noteId);
    Task<PagedResultWithExtras<Note, UserNotesStatsDto>> GetMyNotesAndStatsAsync(MyNotesQueryParams queryParams);
    Task<Result<Note>> GetNoteByIdAsync(string noteId);
    Task<Result<Note>> GetNoteBySlugAsync(string slug);
    Task<PagedResult<Note>> GetNotesAsync(NoteQueryParams parameters);
    Task<Result> ToggleNotePrivacyAsync(string noteId);
    Task<Result> UpdateNoteAsync(string noteId, UpdateNoteRequest request);
}