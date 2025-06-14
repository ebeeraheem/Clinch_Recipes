using CodeStash.Application.Models.Dtos;
using CodeStash.Application.Models.QueryParams;

namespace CodeStash.Application.Contracts;
public interface INoteService
{
    Task<Result<Note>> CreateNoteAsync(CreateNoteRequest request);
    Task<Result> DeleteNoteAsync(string noteId);
    Task<PagedResultWithExtras<Note, UserNotesStatsDto>> GetMyNotesAndStatsAsync(MyNotesQueryParams queryParams);
    Task<Result<Note>> GetNoteByIdAsync(string noteId);
    Task<Result<NoteDetailsDto>> GetNoteBySlugAsync(string slug);
    Task<PagedResult<Note>> GetNotesAsync(NoteQueryParams parameters);
    Task<List<Note>> GetUserPublicNotesAsync(string userName, int pageNumber = 1, int pageSize = 50);
    Task<Result> ToggleNotePrivacyAsync(string noteId);
    Task<Result> UpdateNoteAsync(string noteId, UpdateNoteRequest request);
}