using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Pages.FeedbackPages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(IFeedbackService feedbackService,
                           UserManager<ApplicationUser> userManager)
        {
            _feedbackService = feedbackService;
            _userManager = userManager;
        }

        [BindProperty]
        public FeedbackInputModel Input { get; set; } = new();

        public bool Success { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var feedback = new Feedback
            {
                SenderId = user.Id,
                Message = Input.Message,
                Rating = Input.Rating,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _feedbackService.CreateFeedbackAsync(feedback);

            Success = true;
            ModelState.Clear();
            Input = new FeedbackInputModel();

            return Page();
        }
    }

    public class FeedbackInputModel
    {
        [Required(ErrorMessage = "Please write your feedback before submitting.")]
        [MinLength(10, ErrorMessage = "Feedback must be at least 10 characters.")]
        [MaxLength(500, ErrorMessage = "Feedback cannot exceed 500 characters.")]
        public string Message { get; set; } = null!;

        [Range(1, 5)]
        public int? Rating { get; set; }
    }
}