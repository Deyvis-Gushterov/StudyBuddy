using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Controllers
{
    [Authorize]
    [Route("api/comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ICommentService commentService,
                                  UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }


        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int blogId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Comment cannot be empty.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var comment = new Comment
            {
                Content = content,
                AuthorId = user.Id,
                BlogId = blogId,
                Author = user  // ← set directly, no DB lookup needed
            };

            var result = await _commentService.CreateCommentAsync(comment);
            if (result == null) return BadRequest();

            // Use the original comment object which already has Author set
            // instead of fetching from DB again
            return PartialView("_Comment", comment);
        }


            [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeComment(int id, string targetId,string doerId)
        {
            await _commentService.LikeCommentAsync(id, targetId, doerId);
            var comment = await _commentService.GetCommentByIdAsync(id);
            return Content(comment?.Likes.ToString() ?? "0");
        }


        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user?.Id != comment.AuthorId && !User.IsInRole("Admin"))
                return Forbid();

            await _commentService.DeleteCommentAsync(id);
            return Content("");
        }
    }
}