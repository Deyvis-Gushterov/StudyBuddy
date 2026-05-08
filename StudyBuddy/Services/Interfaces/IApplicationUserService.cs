using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IApplicationUserService
    {
        // Basic queries
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<List<ApplicationUser>> GetAllUsersAsync();

        // Profile
        Task<ApplicationUser?> GetUserWithDetailsAsync(string id);
        Task<bool> UpdateUserAsync(string id, ApplicationUser user);
        Task<bool> DeleteUserAsync(string id);

        // Followers
        Task<bool> FollowUserAsync(string followerId, string targetId);
        Task<bool> UnfollowUserAsync(string followerId, string targetId);
        Task<List<ApplicationUser>> GetFollowersAsync(string userId);

        // Saved Notes
        Task<bool> SaveNoteAsync(string userId, int noteId);
        Task<bool> UnsaveNoteAsync(string userId, int noteId);
        Task<List<Note>> GetSavedNotesAsync(string userId);

    }
}
