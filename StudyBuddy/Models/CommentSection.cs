using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class CommentSection
    {
        [Key]
        public int Id { get; set; }
        public int CommentCount { get; set; }

        public ICollection<Comment> Comments { get; set; }  = new List<Comment>();
    }
}
