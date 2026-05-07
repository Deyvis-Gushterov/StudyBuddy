using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyBuddy.Common;

namespace StudyBuddy.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ReporterId { get; set; } = null!;
        [ForeignKey("ReporterId")]
        public ApplicationUser Reporter { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.MaxReportReasonLength)]
        public string Reason { get; set; } = null!;

        [MinLength(ValidationConstants.MinReportDetailsLength)]
        [MaxLength(ValidationConstants.MaxReportDetailsLength)]
        public string? Details { get;set;  }

        
        public int? BlogId { get; set; }

        
        public Blog? Blog { get; set; }

        
        public int? NoteId { get; set; }

        
        public Note? Note { get; set; }

        
        public int? CommentId { get; set; }

        
        public Comment? Comment { get; set; }

        public bool IsResolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
