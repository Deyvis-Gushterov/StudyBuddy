using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;
namespace StudyBuddy.Models
{
    public class Note
    {
        [Required]
        public int Id { get; set; }

        [MinLength(ValidationConstants.MinTopicLength)]
        [MaxLength(ValidationConstants.MaxTopicLength)]
        public string Topic { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MinExplanationLength)]
        [MaxLength(ValidationConstants.MaxExplanationLength)]
        public string BriefExplanation { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MinExplanationLength)]
        public string Content { get; set; } = null!;
        [Required]
        public string Subject { get; set; } = null!;


        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        [Required]
        public string CreatorId { get; set; } = null!;

        [ForeignKey("CreatorId")]
        public ApplicationUser Creator { get; set; } = null!;

        public int Likes { get; set; }
       


        
    }
}
