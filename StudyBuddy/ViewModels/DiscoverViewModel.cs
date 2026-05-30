using StudyBuddy.Models;

namespace StudyBuddy.ViewModels
{
    public class DiscoverViewModel
    {
        public List<ApplicationUser> SuggestedUsers { get; set; } = new();
        public List<StudyGroup> SuggestedGroups { get; set; } = new();
        public string TopSubject { get; set; } = string.Empty;
    }
}
