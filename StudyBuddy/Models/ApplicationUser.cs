using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Common;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MinLength(ValidationConstants.MinNameLength)]
        [MaxLength(ValidationConstants.MaxNameLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MinNameLength)]
        [MaxLength(ValidationConstants.MaxNameLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        [Range(13, 100)]
        public int Age { get; set; }

        public ICollection<Note> PersonalNotes { get; set; } = new List<Note>();
        public ICollection<Note> SavedNotes { get; set; } = new List<Note>();
        public ICollection<Blog> SavedBlogs { get; set; } = new List<Blog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<ApplicationUser> Followers { get; set; } = new List<ApplicationUser>();
    }
}
