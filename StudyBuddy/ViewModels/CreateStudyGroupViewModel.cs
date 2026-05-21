using StudyBuddy.Common;
using StudyBuddy.Models;
using System.ComponentModel.DataAnnotations;
namespace StudyBuddy.ViewModels
{
    public class CreateStudyGroupViewModel
    {
        [Required]
        [MaxLength(ValidationConstants.MaxGroupNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MinGroupDescriptionLength)]
        [MaxLength(ValidationConstants.MaxGroupDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public Subject Subject { get; set; }
    }
}