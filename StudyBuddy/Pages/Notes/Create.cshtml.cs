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
    public class CreateModel : PageModel
    {
        private readonly INoteService _noteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _userManager = userManager;
        }

        [BindProperty]
        public NoteInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var note = new Note
            {
                Topic = Input.Topic,
                Subject = Input.Subject,
                BriefExplanation = Input.BriefExplanation,
                Content = Input.Content,
                CreatorId = user.Id,
                DateOfCreation = DateTime.Now
            };

            var result = await _noteService.CreateNoteAsync(note);

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return Page();
            }

            return RedirectToPage("/Notes/Index");
        }
    }

    public class NoteInputModel
    {
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
        [MaxLength(200, ErrorMessage = "Content cannot exceed 200 characters.")]
        public string Content { get; set; } = null!;
    }
}
