using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Hubs;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class NotificationService: INotificationService
    {
        private readonly ApplicationDbContext context;
        private readonly IHubContext<NotificationHub> hubContext;
        public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            this.context = context;
            this.hubContext = hubContext;
        }

        public async Task CreateAsync(string recipientId, string authorId, NotificationType type, int? noteId = null, int? blogId = null)
        {

            if (recipientId == authorId) return;

            var recipientExists = await context.Users.AnyAsync(u => u.Id == recipientId);
            var actorExists = await context.Users.AnyAsync(u => u.Id == authorId);

            if (!recipientExists || !actorExists) return;

            var notification = new Notification
            {
                RecipientId = recipientId,
                ActorId = authorId,
                Type = type,
                NoteId = noteId,
                BlogId = blogId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            context.Add(notification);
            await context.SaveChangesAsync();

            var actor = await context.Users.FindAsync(authorId);
            var actorName = actor != null ? $"{actor.FirstName} {actor.LastName}" : "Someone";

            var message = BuildMessage(actorName, type, noteId, blogId);
            var unreadCount = await GetUnreadCountAsync(recipientId);

            await hubContext.Clients.Group(recipientId).SendAsync("ReceiveNotification", new
            {
                id = notification.Id,
                message,
                type = type.ToString(),
                isRead = false,
                createdAt = notification.CreatedAt,
                noteId,
                blogId,
                unreadCount
            });
        }

        public async Task<List<Notification>> GetForUserAsync(string userId, int count = 20)
        {
          return await context.Notifications
                .Where(n => n.RecipientId == userId)
                .Include(n => n.Actor)
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await context.Notifications
                .Where(n => n.RecipientId == userId)
                .Where(n => n.IsRead == false)
                .CountAsync();
        }

        public async Task MarkAsReadAsync(int notification)
        {
            var notific = await context.Notifications
                .Where(n => n.Id == notification)
                .FirstOrDefaultAsync();

            if(notific == null)
            {
                return;
            }

            if (notific.IsRead)
            {
                return;
            }

            else if (!notific.IsRead)
            {
                notific.IsRead = true;
                await context.SaveChangesAsync();
            }

            
        }

        
        public async Task MarkAllReadAsync(string userId)
        {
            var notifications = await context.Notifications
                .Where(n => n.RecipientId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
                notification.IsRead = true;

            await context.SaveChangesAsync();

        }

        private static string BuildMessage(string actorName, NotificationType type, int? noteId, int? blogId)
        {
            return type switch
            {
                NotificationType.NoteLiked => $"{actorName} liked your note",
                NotificationType.BlogLiked => $"{actorName} liked your blog",
                NotificationType.CommentLiked => $"{actorName} liked your comment",
                NotificationType.NewComment => $"{actorName} commented on your blog",
                NotificationType.PostLike => $"{actorName} liked your post",
                NotificationType.NewFollower => $"{actorName} started following you",
                _ => $"{actorName} interacted with your content",
                
            };
        }

    }
}
