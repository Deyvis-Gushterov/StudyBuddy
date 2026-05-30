using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Services.Interfaces;
using System.Security.Claims;

namespace StudyBuddy.Controllers
{
    [Authorize]
    public class DiscoverController : Controller
    {
        private readonly IDiscoverService discoverService;

        public DiscoverController(IDiscoverService discoverService)
        {
            this.discoverService = discoverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var viewModel = await discoverService.GetSuggestionsAsync(userId);
            return View(viewModel);
        }
    }
}
