
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class CommentService: ICommentService
    {
        private readonly ApplicationDbContext context;

        public CommentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await context.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return null;
            }

            return comment;
        }
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await context.Comments
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();
        }
        public async Task<Comment?> CreateCommentAsync(Comment comment)
        {
            if (comment == null)
            {
                return null;
            }

            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            return comment;
        }
        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await context.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Comment?> GetCommentWithOwnerAsync(int id)
        {
            var comment = await context.Comments
                .Include(c => c.Author)
                .Include (c => c.Replies)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return null;
            }

            return comment;
        }
        public async Task<bool> LikeCommentAsync(int id)
        {
            var comment = await context.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            comment.Likes++;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DislikeCommentAsync(int id)
        {
            var comment = await context.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            comment.Dislikes++;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
