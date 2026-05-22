using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Implementations;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationUserService _userService;
        private readonly INoteService _noteService;
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudyGroupService studyGroupService;

        public IndexModel(
            IApplicationUserService userService,
            INoteService noteService,
            IBlogService blogService,
            UserManager<ApplicationUser> userManager,
            IStudyGroupService studyGroupService)
        {
            _userService = userService;
            _noteService = noteService;
            _blogService = blogService;
            _userManager = userManager;
            this.studyGroupService = studyGroupService;
        }

        public ApplicationUser? User { get; set; }
        public List<Note> MyNotes { get; set; } = new();
        public List<Note> SavedNotes { get; set; } = new();
        public List<Blog> MyBlogs { get; set; } = new();
        public List<ApplicationUser> Followers { get; set; } = new();
        public List<StudyGroup> MyGroups { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(base.User);
            if (currentUser == null)
                return RedirectToPage("/Account/Login");

            MyGroups = await studyGroupService.GetUserGroupsAsync(currentUser.Id);

            User = await _userService.GetUserWithDetailsAsync(currentUser.Id);

            var allNotes = await _noteService.GetAllNotesAsync();
            MyNotes = allNotes.Where(n => n.CreatorId == currentUser.Id).ToList();

            SavedNotes = await _userService.GetSavedNotesAsync(currentUser.Id);

            var allBlogs = await _blogService.GetAllBlogsAsync();
            MyBlogs = allBlogs.Where(b => b.AuthorId == currentUser.Id).ToList();

            Followers = await _userService.GetFollowersAsync(currentUser.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteNoteAsync(int noteId)
        {
            var currentUser = await _userManager.GetUserAsync(base.User);
            var note = await _noteService.GetNoteByIdAsync(noteId);

            if (note == null) return NotFound();

            bool isOwner = currentUser?.Id == note.CreatorId;
            bool isAdmin = base.User.IsInRole("Admin");

            if (!isOwner && !isAdmin) return Forbid();

            await _noteService.DeleteNoteAsync(noteId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnsaveNoteAsync(int noteId)
        {
            var currentUser = await _userManager.GetUserAsync(base.User);
            if (currentUser == null) return RedirectToPage();

            await _userService.UnsaveNoteAsync(currentUser.Id, noteId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteBlogAsync(int blogId)
        {
            var currentUser = await _userManager.GetUserAsync(base.User);
            var blog = await _blogService.GetBlogByIdAsync(blogId);

            if (blog == null) return NotFound();

            bool isOwner = currentUser?.Id == blog.AuthorId;
            bool isAdmin = base.User.IsInRole("Admin");

            if (!isOwner && !isAdmin) return Forbid();

            await _blogService.DeleteBlogAsync(blogId);
            return RedirectToPage();
        }
    }
}
