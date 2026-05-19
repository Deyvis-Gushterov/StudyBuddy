using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Pages.BlogPages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAiService _aiService;

        public CreateModel(IBlogService blogService,
                           UserManager<ApplicationUser> userManager,
                           ApplicationDbContext context,
                           IAiService aiService)
        {
            _blogService = blogService;
            _userManager = userManager;
            _context = context;
            _aiService = aiService;
        }

        [BindProperty]
        public BlogInputModel Input { get; set; } = new();

        // AI results
        public string? AiSuggestions { get; set; }
        public List<string>? SuggestedTags { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(bool publish = false)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var tags = await ParseTagsAsync(Input.Tags);

            var blog = new Blog
            {
                Title = Input.Title,
                Summary = Input.Summary,
                Content = Input.Content,
                AuthorId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Tags = tags,
                PublishedAt = publish ? DateTime.UtcNow : null
            };

            var result = await _blogService.CreateBlogAsync(blog);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return Page();
            }

            return RedirectToPage("/Blogs/Details", new { id = result.Id });
        }

        public async Task<IActionResult> OnPostSuggestAsync(string contentSnapshot)
        {
            ModelState.Clear(); 

            if (string.IsNullOrWhiteSpace(contentSnapshot))
            {
                AiSuggestions = "Please write some content first before asking for suggestions.";
                return Page();
            }

            AiSuggestions = await _aiService.GetWritingSuggestionsAsync(contentSnapshot);
            return Page();
        }

        public async Task<IActionResult> OnPostSuggestTagsAsync(string contentSnapshot)
        {
            ModelState.Clear(); 

            if (string.IsNullOrWhiteSpace(contentSnapshot))
            {
                SuggestedTags = new List<string> { "Please write some content first." };
                return Page();
            }

            SuggestedTags = await _aiService.SuggestTagsAsync(contentSnapshot);
            return Page();
        }

        private async Task<List<BlogTag>> ParseTagsAsync(string? tagString)
        {
            var tagNames = (tagString ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .Take(5)
                .ToList();

            var tags = new List<BlogTag>();
            foreach (var name in tagNames)
            {
                var existing = _context.BlogTags.FirstOrDefault(t => t.Name == name);
                tags.Add(existing ?? new BlogTag { Name = name });
            }

            return tags;
        }
    }

    public class BlogInputModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Summary is required.")]
        [MinLength(20, ErrorMessage = "Summary must be at least 20 characters.")]
        [MaxLength(300, ErrorMessage = "Summary cannot exceed 300 characters.")]
        public string Summary { get; set; } = null!;

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(50, ErrorMessage = "Content must be at least 50 characters.")]
        [MaxLength(10000, ErrorMessage = "Content cannot exceed 10,000 characters.")]
        public string Content { get; set; } = null!;

        public string? Tags { get; set; }
    }
}
