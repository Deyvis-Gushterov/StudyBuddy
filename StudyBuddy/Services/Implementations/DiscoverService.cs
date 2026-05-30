using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;
using StudyBuddy.ViewModels;
using System.Runtime.ExceptionServices;

namespace StudyBuddy.Services.Implementations
{
    public class DiscoverService: IDiscoverService
    {
        private ApplicationDbContext context;

        public DiscoverService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<DiscoverViewModel> GetSuggestionsAsync(string userId)
        {
            var profile = new Dictionary<string, int>();

            // 1. Notes created (+5)
            var createdNotes = await context.Notes
                .Where(n => n.CreatorId == userId)
                .ToListAsync();

            foreach (var note in createdNotes)
                AddPoints(profile, note.Subject, 5);

            // 2. Saved notes (+3) and liked notes (+2)
            var currentUser = await context.Users
                .Include(u => u.SavedNotes)
                .Include(u => u.PersonalNotes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            foreach (var note in currentUser?.SavedNotes ?? new List<Note>())
                AddPoints(profile, note.Subject, 3);


            // 4. Groups joined (+4)
            var joinedGroups = await context.StudyGroupMembers
                .Where(m => m.UserId == userId)
                .Include(m => m.StudyGroup)
                .ToListAsync();

            foreach (var member in joinedGroups)
                AddPoints(profile, member.StudyGroup.Subject.ToString(), 4);

            if (!profile.Any())
                return new DiscoverViewModel();

            var topSubject = profile
                .OrderByDescending(x => x.Value)
                .First()
                .Key;

            // Suggest users who have created notes in the top subject
            var suggestedUsers = await context.Notes
                .Where(n => n.Subject == topSubject && n.CreatorId != userId)
                .Include(n => n.Creator)
                .Select(n => n.Creator)
                .Distinct()
                .Take(10)
                .ToListAsync();

            // Suggest groups matching top subject that user hasn't joined
            var joinedGroupIds = joinedGroups.Select(m => m.StudyGroupId).ToHashSet();

            var suggestedGroups = await context.StudyGroups
                .Where(g => g.Subject.ToString() == topSubject && !joinedGroupIds.Contains(g.Id))
                .Include(g => g.Members)
                .OrderByDescending(g => g.Members.Count)
                .Take(6)
                .ToListAsync();

            return new DiscoverViewModel
            {
                SuggestedUsers = suggestedUsers,
                SuggestedGroups = suggestedGroups,
                TopSubject = topSubject
            };
        }

        private void AddPoints(Dictionary<string, int> profile, string subject, int points)
        {
            if (!profile.ContainsKey(subject))
            {
                profile[subject] = 0;
            }
                profile[subject] += points;
        }
    }
}
