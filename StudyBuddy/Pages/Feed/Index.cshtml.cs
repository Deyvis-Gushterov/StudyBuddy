using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Implementations;
using StudyBuddy.Services.Interfaces;
using System.Text.RegularExpressions;

namespace StudyBuddy.Pages.Feed
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly ICommentService _commentService;
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public string? Tag { get; set; }
        public string? Search { get; set; }

        public IndexModel(IPostService postService,
                          UserManager<ApplicationUser> userManager,
                          IApplicationUserService userService,
                          IWebHostEnvironment env,
                          ICommentService commentService,
                          CloudinaryDotNet.Cloudinary cloudinary)
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
            _env = env;
            _commentService = commentService;
            _cloudinary = cloudinary;
        }

        public List<Post> Posts { get; set; } = new();
        public List<ApplicationUser> SuggestedUsers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? tag, string? search)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var following = await _userService.GetFollowingAsync(user.Id);
            var followingIds = following.Select(f => f.Id).ToList();
            followingIds.Add(user.Id);

            var allUsers = await _userService.GetAllUsersAsync();
            SuggestedUsers = allUsers
                .Where(u => u.Id != user.Id && !followingIds.Contains(u.Id))
                .Take(5)
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                Search = search;
                var allPosts = await _postService.GetAllPostsAsync();
                Posts = allPosts
                    .Where(p => p.Content.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                p.Tags.Any(t => t.Name.Contains(search, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(tag))
            {
                Tag = tag;
                Posts = await _postService.GetByTagAsync(new PostTag { Name = tag });
            }
            else
            {
                Posts = await _postService.GetFeedAsync(followingIds);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string content, IFormFile? image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            if (string.IsNullOrWhiteSpace(content) && image == null)
                return RedirectToPage();

            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext)) return RedirectToPage();
                if (image.Length > 5 * 1024 * 1024) return RedirectToPage();

                // WITH THIS:
                using var stream = image.OpenReadStream();
                var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
                {
                    File = new CloudinaryDotNet.FileDescription(image.FileName, stream),
                    Folder = "studybuddy/posts"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageUrl = uploadResult.SecureUrl.ToString();

                // Extract hashtags from content
                var tags = new List<PostTag>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var matches = Regex.Matches(content, @"#(\w+)");
                var tagList = matches.Select(m => m.Groups[1].Value.ToLower()).Distinct().ToList();
                foreach (var tag in tagList)
                    tags.Add(new PostTag { Name = tag });
            }

            var post = new Post
            {
                Content = content ?? "",
                AuthorId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = imageUrl,
                Tags = tags
            };

            await _postService.CreatePostAsync(post);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id, string targetId, string doerId)
        {
            await _postService.LikeAsync(id, targetId, doerId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCommentAsync(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return RedirectToPage();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            await _commentService.CreatePostCommentAsync(postId, content, user.Id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        bool isOwner = user?.Id == post.AuthorId;
        bool isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin) return Forbid();

            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var filePath = Path.Combine(_env.WebRootPath, post.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            await _postService.DeletePostAsync(id);
            return RedirectToPage();
    }
}
}
