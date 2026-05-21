using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.BlogPages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogService;
        private const int PageSize = 9;

        public IndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public List<Blog> Blogs { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Search { get; set; }

        public async Task OnGetAsync(string? search, int page = 1)
        {
            Search = search;
            CurrentPage = page;

            var (items, total) = await _blogService.GetPagedAsync(search, page, PageSize);
            Blogs = items;
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        }
    }
}