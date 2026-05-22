using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;

namespace StudyBuddy.Models
{
    public class StudyGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxGroupNameLength)]
        public string Name { get; set; } = null!;

        [MinLength(ValidationConstants.MinGroupDescriptionLength)]
        [MaxLength(ValidationConstants.MaxGroupDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public Subject Subject { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatorId { get; set; } = null!;
        [ForeignKey("CreatorId")]
        public ApplicationUser Creator { get; set; } = null!;
        public ICollection<StudyGroupMember> Members { get; set; } = new List<StudyGroupMember>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }

    public enum Subject
    {
        Math,
        Science,
        History,
        Literature,
        ComputerScience,
        Physics,
        Chemistry,
        Biology,
        Economics,
        Other
    }
}
