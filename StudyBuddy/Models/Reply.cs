using StudyBuddy.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.Models
{
    public class Reply
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
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; } = null!;
    }
}
