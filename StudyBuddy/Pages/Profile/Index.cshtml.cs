using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.UserProfile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationUserService _userService;
        private readonly INoteService _noteService;
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            IApplicationUserService userService,
            INoteService noteService,
            IBlogService blogService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _noteService = noteService;
            _blogService = blogService;
            _userManager = userManager;
        }

        public ApplicationUser? ProfileUser { get; set; }
        public List<Note> UserNotes { get; set; } = new();
        public List<Blog> UserBlogs { get; set; } = new();
        public List<ApplicationUser> Followers { get; set; } = new();
        public bool IsOwnProfile { get; set; }
        public bool IsFollowing { get; set; }
        public string? CurrentUserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Dashboard/Index");

            ProfileUser = await _userService.GetUserWithDetailsAsync(userId);
            if (ProfileUser == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            CurrentUserId = currentUser?.Id;
            IsOwnProfile = currentUser?.Id == userId;

            // Get user's notes and blogs
            var allNotes = await _noteService.GetAllNotesAsync();
            UserNotes = allNotes.Where(n => n.CreatorId == userId).ToList();

            var allBlogs = await _blogService.GetAllBlogsAsync();
            UserBlogs = allBlogs
                .Where(b => b.AuthorId == userId && b.PublishedAt != null)
                .OrderByDescending(b => b.PublishedAt)
                .ToList();

            Followers = await _userService.GetFollowersAsync(userId);
            IsFollowing = Followers.Any(f => f.Id == currentUser?.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostFollowAsync(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage(new { userId });

            await _userService.FollowUserAsync(currentUser.Id, userId);
            return RedirectToPage(new { userId });
        }

        public async Task<IActionResult> OnPostUnfollowAsync(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage(new { userId });

            await _userService.UnfollowUserAsync(currentUser.Id, userId);
            return RedirectToPage(new { userId });
        }
    }
}