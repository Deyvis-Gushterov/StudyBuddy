using StudyBuddy.Models;

namespace StudyBuddy.ViewModels
{
    public class StudyGroupDetailsViewModel
    {
        public StudyGroup Group { get; set; } = null!;
        public bool IsMember { get; set; }
        public bool IsCreator { get; set; }
        public List<Note> UserNotes { get; set; } = new();
        public List<Blog> UserBlogs { get; set; } = new();
        public string CurrentUserId { get; set; } = string.Empty;
    }
}