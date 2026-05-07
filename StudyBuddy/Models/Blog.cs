using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;

namespace StudyBuddy.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinBlogTitleLength)]
        [MaxLength(ValidationConstants.MaxBlogTitleLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(ValidationConstants.MinContentLength)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string AuthorId { get; set; } = null!;

        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; } = null!;
        public string Summary { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        public int Views { get; set; }
        public int Likes { get; set; }

        public ICollection<BlogTag> Tags { get; set; } = new List<BlogTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
