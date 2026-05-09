using StudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.Notes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly INoteService _noteService;

        public IndexModel(INoteService noteService)
        {
            _noteService = noteService;
        }

        public List<Note> Notes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Notes = await _noteService.GetAllNotesAsync();
        }
    }
}
