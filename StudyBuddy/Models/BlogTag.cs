using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class BlogTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
