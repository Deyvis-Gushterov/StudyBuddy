using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;

namespace StudyBuddy.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderId { get; set; } = null!;

        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MinFeedbackMessageLength)]
        [MaxLength(ValidationConstants.MaxFeedbackMessageLength)]
        public string Message {  get; set; } = null!;

        [Range(1, 5)]
        public int? Rating {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
