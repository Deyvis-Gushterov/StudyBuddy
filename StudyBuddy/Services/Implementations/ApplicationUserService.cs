using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class ApplicationUserService:IApplicationUserService
    {
        private readonly ApplicationDbContext context;
        private readonly INotificationService notificationService;
        public ApplicationUserService(ApplicationDbContext context, INotificationService notificationService)
        {
            this.context = context;
            this.notificationService = notificationService;
        }

        // Basic queries
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return user;
        }
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var user = await context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user;
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await context.Users
                .ToListAsync();
        }

        // Profile
        public async Task<ApplicationUser?> GetUserWithDetailsAsync(string id)
        {
            var user = await context.Users
                .Include(u => u.Followers)
                .Include(u => u.PersonalNotes)
                .Include(u => u.SavedNotes)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user;
        }
        public async Task<bool> UpdateUserAsync(string id, ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }

            var oldUser = await context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (oldUser == null)
            {
                return false;
            }

            oldUser.UserName = user.UserName;
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Email = user.Email;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.Nationality = user.Nationality;

            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        // Followers
        public async Task<bool> FollowUserAsync(string followerId, string targetId)
        {
            var follower = await context.Users
                .FirstOrDefaultAsync(u => u.Id == followerId);

            var target = await context.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == targetId);

            if (follower == null || target == null)
            {
                return false;
            }

            if (target.Followers.Any(u => u.Id == followerId))
            {
                return false; 
            }

            target.Followers.Add(follower);
            await context.SaveChangesAsync();

            await notificationService.CreateAsync(
        recipientId: targetId,   // the person being followed
        authorId: followerId,     // the person who followed
        type: NotificationType.NewFollower
    );
            return true;
        }
        public async Task<bool> UnfollowUserAsync(string followerId, string targetId)
        {
            var follower = await context.Users
                .FirstOrDefaultAsync(u => u.Id == followerId);

            var target = await context.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == targetId);

            if (follower == null || target == null)
            {
                return false;
            }

            if (!target.Followers.Any(u => u.Id == followerId))
            {
                return false;
            }

            target.Followers.Remove(follower);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ApplicationUser>> GetFollowersAsync(string userId)
        {
            var user = await context.Users
         .Include(u => u.Followers)
         .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return new List<ApplicationUser>();

            return user.Followers.ToList();
        }

        // Saved Notes
        public async Task<bool> SaveNoteAsync(string userId, int noteId)
        {
            var user = await context.Users
                .Include(u => u.SavedNotes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var note = await context.Notes
                .FirstOrDefaultAsync(u => u.Id == noteId);

            if (user == null)
            {
                return false;
            }

            if (note == null)
            {
                return false;
            }

            if (user.SavedNotes.Contains(note))
            {
                return false;
            }

            user.SavedNotes.Add(note);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnsaveNoteAsync(string userId, int noteId)
        {
            var user = await context.Users
                .Include(u => u.SavedNotes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var note = await context.Notes
                .FirstOrDefaultAsync(u => u.Id == noteId);

            if (user == null)
            {
                return false;
            }

            if (note == null)
            {
                return false;
            }
            
            user.SavedNotes.Remove(note);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Note>> GetSavedNotesAsync(string userId)
        {
            var user = await context.Users
                .Include(u => u.SavedNotes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new List<Note>();
            }

            return user.SavedNotes.ToList();
        }

        public async Task<bool> SaveBlogAsync(string userId, int blogId)
        {
            var user = await context.Users
                .Include(u => u.SavedBlogs)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var blog = await context.Blogs
                .FirstOrDefaultAsync(u => u.Id == blogId);

            if (user == null)
            {
                return false;
            }

            if (blog == null)
            {
                return false;
            }

            if (user.SavedBlogs.Contains(blog))
            {
                return false;
            }

            user.SavedBlogs.Add(blog);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnsaveBlogAsync(string userId, int blogId)
        {
            var user = await context.Users
                .Include(u => u.SavedBlogs)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var blog = await context.Blogs
                .FirstOrDefaultAsync(u => u.Id == blogId);

            if (user == null)
            {
                return false;
            }

            if (blog == null)
            {
                return false;
            }

            user.SavedBlogs.Remove(blog);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Blog>> GetSavedBlogsAsync(string userId)
        {
            var user = await context.Users
                .Include(u => u.SavedBlogs)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new List<Blog>();
            }

            return user.SavedBlogs.ToList();
        }

        public async Task<List<ApplicationUser>> GetFollowingAsync(string userId)
        {
            return await context.Users
                .Where(u => u.Followers.Any(f => f.Id == userId))
                .ToListAsync();
        }
    }
}
