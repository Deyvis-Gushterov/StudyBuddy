using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.NotePages
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly INoteService _noteService;
        private readonly IApplicationUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(INoteService noteService,
                            IApplicationUserService userService,
                            UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _userService = userService;
            _userManager = userManager;
        }

        public Note? Note { get; set; }
        public bool IsOwner { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Note = await _noteService.GetNoteWithOwnerAsync(id);

            if (Note == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            IsOwner = currentUser?.Id == Note.CreatorId;

            return Page();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id)
        {
            await _noteService.LikeAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostSaveAsync(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
                await _userService.SaveNoteAsync(currentUser.Id, id);

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            bool isAdmin = User.IsInRole("Admin");
            bool isOwner = currentUser?.Id == note.CreatorId;

            if (!isOwner && !isAdmin)
                return Forbid();

            await _noteService.DeleteNoteAsync(id);
            return RedirectToPage("/Notes/Index");
        }
    }
}
