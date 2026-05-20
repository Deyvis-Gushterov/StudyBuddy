using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;

namespace StudyBuddy.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(ValidationConstants.MinCommentContentLength)]
        [MaxLength(ValidationConstants.MaxCommentContentLength)]
        public string Content { get; set; } = null!;
        [Required]
        public string AuthorId { get; set; } = null!;
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; } = null!;
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; } = null!;

        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    }
}
