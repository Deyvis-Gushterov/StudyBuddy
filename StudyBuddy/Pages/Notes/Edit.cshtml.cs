using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Pages.NotePages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly INoteService _noteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _userManager = userManager;
        }

        public Note? Note { get; set; }

        [BindProperty]
        public NoteEditInputModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Note = await _noteService.GetNoteByIdAsync(id);

            if (Note == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            bool isOwner = currentUser?.Id == Note.CreatorId;
            bool isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin)
                return Forbid();

            // Pre-populate the form
            Input = new NoteEditInputModel
            {
                Id = Note.Id,
                Topic = Note.Topic,
                Subject = Note.Subject,
                BriefExplanation = Note.BriefExplanation,
                Content = Note.Content
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Note = await _noteService.GetNoteByIdAsync(Input.Id);

            if (Note == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            bool isOwner = currentUser?.Id == Note.CreatorId;
            bool isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin)
                return Forbid();

            if (!ModelState.IsValid)
                return Page();

            var updated = new Note
            {
                Topic = Input.Topic,
                Subject = Input.Subject,
                BriefExplanation = Input.BriefExplanation,
                Content = Input.Content
            };

            await _noteService.UpdateNoteAsync(Input.Id, updated);

            return RedirectToPage("/Notes/Details", new { id = Input.Id });
        }
    }

    public class NoteEditInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Topic is required.")]
        [MinLength(3, ErrorMessage = "Topic must be at least 3 characters.")]
        [MaxLength(100, ErrorMessage = "Topic cannot exceed 100 characters.")]
        public string Topic { get; set; } = null!;

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Brief explanation is required.")]
        [MinLength(20, ErrorMessage = "Brief explanation must be at least 20 characters.")]
        [MaxLength(200, ErrorMessage = "Brief explanation cannot exceed 200 characters.")]
        public string BriefExplanation { get; set; } = null!;

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(20, ErrorMessage = "Content must be at least 20 characters.")]
        [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters.")]
        public string Content { get; set; } = null!;
    }
}
