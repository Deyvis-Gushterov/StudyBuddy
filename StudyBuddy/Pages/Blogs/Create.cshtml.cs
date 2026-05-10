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

        public CreateModel(IBlogService blogService,
                           UserManager<ApplicationUser> userManager,
                           ApplicationDbContext context)
        {
            _blogService = blogService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public BlogInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(bool publish = false)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            // Parse tags from comma-separated hidden input
            var tagNames = (Input.Tags ?? "")
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
                if (existing != null)
                    tags.Add(existing);
                else
                    tags.Add(new BlogTag { Name = name });
            }

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

        // Comma-separated tag names from the tag chip UI
        public string? Tags { get; set; }
    }
}