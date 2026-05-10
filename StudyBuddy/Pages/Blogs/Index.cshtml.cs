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

        public IndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public List<Blog> Blogs { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _blogService.GetAllBlogsAsync();
            // Only show published blogs on the public index
            Blogs = all
                .Where(b => b.PublishedAt != null)
                .OrderByDescending(b => b.PublishedAt)
                .ToList();
        }
    }
}