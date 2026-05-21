using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.Models
{
    public class StudyGroupMember
    {
        [Required]
        public int StudyGroupId { get; set; }

        [ForeignKey("StudyGroupId")]
        public StudyGroup StudyGroup { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin, 
        Member
    }
}
