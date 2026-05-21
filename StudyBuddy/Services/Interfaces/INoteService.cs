using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note?> GetNoteByIdAsync(int id);
        Task<List<Note>> GetAllNotesAsync();
        Task<Note?> CreateNoteAsync(Note note);
        Task<Note?> UpdateNoteAsync(int id, Note note);
        Task<bool> DeleteNoteAsync(int id);
        Task<Note?> GetNoteWithOwnerAsync(int id);
        Task<bool> LikeAsync(int id, string targetId, string doerId);
        Task<(List<Note> Items, int TotalCount)> GetPagedAsync(string? search, string? subject, int page, int pageSize);
    }
}
