using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IPostService
    {
        // Feed
        Task<List<Post>> GetFeedAsync(List<string> followingIds, int count = 20);
        Task<List<Post>> GetAllPostsAsync();
        Task<List <Post>> GetByTagAsync(PostTag tag);

        // CRUD
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post?> GetPostWithAuthorAsync(int id);
        Task<Post?> CreatePostAsync(Post post);
        Task<bool> DeletePostAsync(int id);

        // Social
        Task<bool> LikeAsync(int id, string targetId, string doerId);

        // Profile
        Task<List<Post>> GetPostsByUserAsync(string userId);
    }
}
