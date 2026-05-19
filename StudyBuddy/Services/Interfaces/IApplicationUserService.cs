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
        Task<List<ApplicationUser>> GetFollowingAsync(string userId);
        Task<List<ApplicationUser>> GetFollowersAsync(string userId);

        // Saved Notes
        Task<bool> SaveNoteAsync(string userId, int noteId);
        Task<bool> UnsaveNoteAsync(string userId, int noteId);
        Task<List<Note>> GetSavedNotesAsync(string userId);

        //Saved Blogs
        Task<bool> SaveBlogAsync(string userId, int blogId);
        Task<bool> UnsaveBlogAsync(string userId, int blogId);
        Task<List<Blog>> GetSavedBlogsAsync(string userId);

    }
}
