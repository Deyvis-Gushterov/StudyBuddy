using StudyBuddy.Models;

namespace StudyBuddy.ViewModels
{
    public class StudyGroupDetailsViewModel
    {
        public StudyGroup Group { get; set; } = null!;
        public bool IsMember { get; set; }
        public bool IsCreator { get; set; }
    }
}