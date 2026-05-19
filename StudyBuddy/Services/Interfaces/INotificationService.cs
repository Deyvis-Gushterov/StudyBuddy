using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateAsync(string recipientId, string authorId, NotificationType type, int? noteId = null, int? blogId = null);
        Task<List<Notification>> GetForUserAsync(string userId, int count = 20);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int notification);
        Task MarkAllReadAsync(string userId);
    }
}
