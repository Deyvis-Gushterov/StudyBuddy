using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<Feedback?> GetFeedbackByIdAsync(int id);
        Task<List<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback?> CreateFeedbackAsync(Feedback feedback);
        Task<bool> DeleteFeedbackAsync(int id);
        Task<Feedback?> GetFeedbackWithOwnerAsync(int id);
        Task<bool> MarkAsReadAsync(int id);

    }
}
