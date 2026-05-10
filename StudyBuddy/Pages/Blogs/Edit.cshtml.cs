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
    public class EditModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EditModel(IBlogService blogService,
                         UserManager<ApplicationUser> userManager,
                         ApplicationDbContext context)
        {
            _blogService = blogService;
            _userManager = userManager;
            _context = context;
        }

        public Blog? Blog { get; set; }

        [BindProperty]
        public BlogEditInputModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Blog = await _blogService.GetBlogWithOwnerAsync(id);
            if (Blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != Blog.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            Input = new BlogEditInputModel
            {
                Id = Blog.Id,
                Title = Blog.Title,
                Summary = Blog.Summary,
                Content = Blog.Content,
                Tags = string.Join(',', Blog.Tags.Select(t => t.Name))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Blog = await _blogService.GetBlogWithOwnerAsync(Input.Id);
            if (Blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != Blog.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            if (!ModelState.IsValid)
                return Page();

            // Parse tags
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
                tags.Add(existing ?? new BlogTag { Name = name });
            }

            Blog.Title = Input.Title;
            Blog.Summary = Input.Summary;
            Blog.Content = Input.Content;
            Blog.Tags = tags;
            Blog.UpdatedAt = DateTime.UtcNow;

            await _blogService.UpdateBlogAsync(Input.Id, Blog);
            return RedirectToPage("/Blogs/Details", new { id = Input.Id });
        }
    }

    public class BlogEditInputModel
    {
        public int Id { get; set; }

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
