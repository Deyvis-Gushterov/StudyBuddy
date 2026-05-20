using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> CreateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
        Task<Comment?> GetCommentWithOwnerAsync(int id);
        Task<bool> LikeCommentAsync(int id);
        Task<bool> DislikeCommentAsync(int id);

        // Post comments
        Task<Comment?> CreatePostCommentAsync(int postId, string content, string authorId);
        Task<List<Comment>> GetPostCommentsAsync(int postId);
    }
}
