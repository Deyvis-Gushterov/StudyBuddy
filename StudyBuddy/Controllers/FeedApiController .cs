using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Controllers;

[ApiController]
[Route("api/feed")]
public class FeedApiController : ControllerBase
{
    private readonly IPostService _postService;

    public FeedApiController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeed()
    {
        var posts = await _postService.GetAllPostsAsync();

        var result = posts.Select(p => new
        {
            id = p.Id,
            content = p.Content,
            likes = p.Likes,
            createdAt = p.CreatedAt,
            authorName = p.Author != null ? $"{p.Author.FirstName} {p.Author.LastName}" : "Unknown"
        });

        return Ok(result);
    }
}