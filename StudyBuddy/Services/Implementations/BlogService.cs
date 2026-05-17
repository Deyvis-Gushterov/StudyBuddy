using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;


namespace StudyBuddy.Services.Implementations
{
    public class BlogService: IBlogService
    {
        private readonly ApplicationDbContext context;

        public BlogService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IncrementViewAsync(int id)
        {
            var blog = await context.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null) return false;

            blog.Views++;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            var blog = await context.Blogs
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (blog == null)
            {
                return null;
            }

            return blog;
        }
        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await context.Blogs
                .Include(b => b.Author)
                .ToListAsync();
        }
        
        public async Task<Blog?> CreateBlogAsync(Blog blog)
        {
            if (blog == null)
            {
                return null;
            }

            context.Blogs.Add(blog);
            await context.SaveChangesAsync();
            return blog;
        }
        public async Task<Blog?> UpdateBlogAsync(int id, Blog blog)
        {
            if (blog == null)
            {
                return null;
            }

            var oldBlog = await context.Blogs
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (oldBlog == null)
            {
                return null;
            }

            oldBlog.Title = blog.Title;
            oldBlog.Content = blog.Content;
            oldBlog.Summary = blog.Summary;
            oldBlog.UpdatedAt = DateTime.Now;

            await context.SaveChangesAsync();
            return oldBlog;
        }
        public async Task<bool> DeleteBlogAsync(int id)
        {
            var blog = await context.Blogs
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if(blog == null)
            {
                return false;
            }

            context.Blogs.Remove(blog);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Blog?> GetBlogWithOwnerAsync(int id)
        {
            var blog = await context.Blogs
                .Include(b => b.Author)
                .Include(b => b.Tags)
                .Include(b => b.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return null;
            }

            return blog;
        }
        public async Task<bool> PublishBlogAsync(int id)
        {
            var blog = await context.Blogs
        .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return false;
            }

            blog.PublishedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnpublishBlogAsync(int id)
        {
            var blog = await context.Blogs
        .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return false;
            }

            blog.PublishedAt = null;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LikeBlogAsync(int id)
        {
            var blog = await context.Blogs
        .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return false;
            }

            blog.Likes++;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
