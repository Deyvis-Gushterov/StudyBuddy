using StudyBuddy.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RecipientId { get; set; } = null!;
        [ForeignKey("RecipientId")]
        public ApplicationUser Recipient { get; set; } = null!;

        [Required]
        public string ActorId { get; set; } = null!;
        [ForeignKey("ActorId")]
        public ApplicationUser Actor { get; set; } = null!;

        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? NoteId { get; set; }
        public int? BlogId { get; set; }
        public int? CommentId { get; set; }
    }

    public enum NotificationType
    {
        NoteLiked,
        BlogLiked,
        CommentLiked,
        NewComment,
        NewFollower,
        PostLike
    }
}
