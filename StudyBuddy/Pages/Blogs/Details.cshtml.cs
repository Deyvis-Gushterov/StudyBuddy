using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.BlogPages
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(IBlogService blogService,
                            ICommentService commentService,
                            UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public Blog? Blog { get; set; }
        public bool IsOwner { get; set; }
        public string? CurrentUserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Blog = await _blogService.GetBlogWithOwnerAsync(id);
            if (Blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            CurrentUserId = currentUser?.Id;
            IsOwner = currentUser?.Id == Blog.AuthorId;

            // Increment view count
            Blog.Views++;
            await _blogService.UpdateBlogAsync(id, Blog);

            return Page();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id)
        {
            await _blogService.LikeBlogAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostPublishAsync(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != blog.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            await _blogService.PublishBlogAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostUnpublishAsync(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != blog.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            await _blogService.UnpublishBlogAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != blog.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            await _blogService.DeleteBlogAsync(id);
            return RedirectToPage("/Blogs/Index");
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int blogId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return RedirectToPage(new { id = blogId });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage(new { id = blogId });

            var comment = new Comment
            {
                Content = content,
                AuthorId = currentUser.Id,
                BlogId = blogId
            };

            await _commentService.CreateCommentAsync(comment);
            return RedirectToPage(new { id = blogId });
        }

        public async Task<IActionResult> OnPostLikeCommentAsync(int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null) return NotFound();

            await _commentService.LikeCommentAsync(commentId);
            return RedirectToPage(new { id = comment.BlogId });
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id != comment.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            int blogId = comment.BlogId;
            await _commentService.DeleteCommentAsync(commentId);
            return RedirectToPage(new { id = blogId });
        }
    }
}
