using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Blog?> GetBlogByIdAsync(int id);
        Task<List<Blog>> GetAllBlogsAsync();
        Task<Blog?> CreateBlogAsync(Blog blog);
        Task<Blog?> UpdateBlogAsync(int id, Blog blog);
        Task<bool> DeleteBlogAsync(int id);
        Task<Blog?> GetBlogWithOwnerAsync(int id);
        Task<bool> PublishBlogAsync(int id);
        Task<bool> UnpublishBlogAsync(int id);
        Task<bool> LikeBlogAsync(int id);

        Task<bool> IncrementViewAsync(int id);
    }
}
