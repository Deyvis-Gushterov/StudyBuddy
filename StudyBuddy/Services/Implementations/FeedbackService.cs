using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class FeedbackService: IFeedbackService
    {
        private readonly ApplicationDbContext context;

        public FeedbackService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int id)
        {
            var feedback = await context.Feedbacks
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (feedback == null)
            {
                return null;
            }

            return feedback;
        }
        public async Task<List<Feedback>> GetAllFeedbacksAsync()
        {
            return await context.Feedbacks
                .Include(f => f.Sender)
                .ToListAsync();
        }
        public async Task<Feedback?> CreateFeedbackAsync(Feedback feedback)
        {
            if (feedback == null)
            {
                return null;
            }

            context.Feedbacks.Add(feedback);
            await context.SaveChangesAsync();
            return feedback;
        }
        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            var feedback = await context.Feedbacks
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (feedback == null)
            {
                return false;
            }

            context.Feedbacks.Remove(feedback);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Feedback?> GetFeedbackWithOwnerAsync(int id)
        {
            var feedback = await context.Feedbacks
                .Include(f => f.Sender)
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (feedback == null)
            {
                return null;
            }

            return feedback;
        }
        public async Task<bool> MarkAsReadAsync(int id)
        {
            var feedback = await context.Feedbacks
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (feedback == null) return false;

            feedback.IsRead = true;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
