using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(300)]
        public string Content { get; set; } = null!;
        [Required]
        public string AuthorId { get; set;} = null!;
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Likes {  get; set; }

        public string? ImageUrl { get; set; }

        public string? Tags { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
