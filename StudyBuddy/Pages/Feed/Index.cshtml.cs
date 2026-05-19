using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Pages.Feed  
{
    [Authorize] 
    public class IndexModel : PageModel  // always extends PageModel
    {
        
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserService _userService;

        public IndexModel(IPostService postService,
                          UserManager<ApplicationUser> userManager,
                          IApplicationUserService userService)
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
        }

        // 2. Properties the Razor page can read
        public List<Post> Posts { get; set; } = new();
        public List<ApplicationUser> SuggestedUsers { get; set; } = new();

        // 3. OnGetAsync = runs when page loads 
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");


            var following = await _userService.GetFollowingAsync(user.Id);
            var followingIds = following.Select(f => f.Id).ToList();

            followingIds.Add(user.Id);

            Posts = await _postService.GetFeedAsync(followingIds);

            return Page();
        }

        // 4. OnPostXxxAsync = runs when a form posts
        public async Task<IActionResult> OnPostCreateAsync(string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var post = new Post
            {
                Content = content,
                AuthorId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _postService.CreatePostAsync(post);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id, string targetId, string doerId)
        {
            await _postService.LikeAsync(id, targetId, doerId);
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

            await _postService.DeletePostAsync(id);
            return RedirectToPage();
        }
    }
}