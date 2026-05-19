using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Hubs;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class PostService: IPostService
    {
        private readonly ApplicationDbContext context;
        private readonly INotificationService notificationService;
        public PostService(ApplicationDbContext context, INotificationService notificationService)
        {
            this.context = context;
            this.notificationService = notificationService;
        }

        public async Task<List<Post>> GetFeedAsync(List<string> followingIds, int count = 20)
        {
           
            var posts = await context.Posts
                .Where(p => followingIds.Contains(p.AuthorId))
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var post = await context.Posts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return null;
            }

            return post;
        }

        public async Task<Post?> GetPostWithAuthorAsync(int id)
        {
            var post = await context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Author)
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return null;
            }

            return post;
        }

        public async Task<Post?> CreatePostAsync(Post post)
        {
            if (post == null)
            {
                return null;
            }

            context.Posts.Add(post);
            await context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await GetPostByIdAsync(id);

            if (post == null)
            {
                return false;
            }

            context.Posts.Remove(post);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LikeAsync(int id, string targetId, string doerId)
        {
            var post = await GetPostByIdAsync(id);

            if (post == null)
            {
                return false;
            }

            if (targetId == null)
            {
                return false;
            }

            if (doerId == null)
            {
                return false;
            }

            post.Likes++;

            await context.SaveChangesAsync();

            await notificationService.CreateAsync(
                recipientId: targetId,
                authorId: doerId,
                type: NotificationType.PostLike
            );

            return true;
        }

        public async Task<List<Post>> GetPostsByUserAsync(string userId)
        {
            return await context.Posts
                 .Where(p => p.AuthorId == userId)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
