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
        private const int PageSize = 9;

        public IndexModel(INoteService noteService)
        {
            _noteService = noteService;
        }

        public List<Note> Notes { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Search { get; set; }
        public string? Subject { get; set; }

        public async Task OnGetAsync(string? search, string? subject, int page = 1)
        {
            Search = search;
            Subject = subject;
            CurrentPage = page;

            var (items, total) = await _noteService.GetPagedAsync(search, subject, page, PageSize);
            Notes = items;
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        }
    }
}