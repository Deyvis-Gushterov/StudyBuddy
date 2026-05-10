using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IApplicationUserService _userService;
        private readonly INoteService _noteService;
        private readonly IBlogService _blogService;
        private readonly IReportService _reportService;
        private readonly IFeedbackService _feedbackService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            IApplicationUserService userService,
            INoteService noteService,
            IBlogService blogService,
            IReportService reportService,
            IFeedbackService feedbackService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _noteService = noteService;
            _blogService = blogService;
            _reportService = reportService;
            _feedbackService = feedbackService;
            _userManager = userManager;
        }

        public List<ApplicationUser> Users { get; set; } = new();
        public List<Note> Notes { get; set; } = new();
        public List<Blog> Blogs { get; set; } = new();
        public List<Report> Reports { get; set; } = new();
        public List<Feedback> Feedbacks { get; set; } = new();
        public Dictionary<string, string> UserRoles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsersAsync();
            Notes = await _noteService.GetAllNotesAsync();
            Blogs = await _blogService.GetAllBlogsAsync();
            Reports = await _reportService.GetAllReportsAsync();
            Feedbacks = await _feedbackService.GetAllFeedbacksAsync();

            // Build role map for display
            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id] = roles.Contains("Admin") ? "Admin" : "User";
            }
        }

        // ── Users ──
        public async Task<IActionResult> OnPostPromoteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToPage();
        }

        // ── Reports ──
        public async Task<IActionResult> OnPostResolveReportAsync(int reportId)
        {
            await _reportService.ResolveReportAsync(reportId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteReportAsync(int reportId)
        {
            await _reportService.DeleteReportAsync(reportId);
            return RedirectToPage();
        }

        // ── Feedback ──
        public async Task<IActionResult> OnPostMarkFeedbackReadAsync(int feedbackId)
        {
            await _feedbackService.MarkAsReadAsync(feedbackId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteFeedbackAsync(int feedbackId)
        {
            await _feedbackService.DeleteFeedbackAsync(feedbackId);
            return RedirectToPage();
        }

        // ── Notes ──
        public async Task<IActionResult> OnPostDeleteNoteAsync(int noteId)
        {
            await _noteService.DeleteNoteAsync(noteId);
            return RedirectToPage();
        }

        // ── Blogs ──
        public async Task<IActionResult> OnPostPublishBlogAsync(int blogId)
        {
            await _blogService.PublishBlogAsync(blogId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnpublishBlogAsync(int blogId)
        {
            await _blogService.UnpublishBlogAsync(blogId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteBlogAsync(int blogId)
        {
            await _blogService.DeleteBlogAsync(blogId);
            return RedirectToPage();
        }
    }
}
