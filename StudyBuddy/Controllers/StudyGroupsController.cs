using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;
using System.Security.Claims;
using StudyBuddy.ViewModels;

namespace StudyBuddy.Controllers
{
    [Authorize]
    public class StudyGroupsController: Controller
    {
        private readonly IStudyGroupService studyGroupService;
        private readonly UserManager<ApplicationUser> userManager;

        public StudyGroupsController(IStudyGroupService studygroupService,
                                          UserManager<ApplicationUser> userManager)
        {
            this.studyGroupService = studygroupService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var result = await studyGroupService.GetAllGroupsAsync();

            if (result == null) return BadRequest();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var group = await studyGroupService.GetByIdAsync(id);

            if (group == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = new StudyGroupDetailsViewModel
            {
                Group = group,
                IsMember = await studyGroupService.IsUserMemberAsync(group.Id, userId!),
                IsCreator = group.CreatorId == userId
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var group = new StudyGroup
            {
                Name = model.Name,
                Description = model.Description,
                Subject = model.Subject,
                CreatedAt = DateTime.UtcNow
            };

            var created = await studyGroupService.CreateGroupAsync(group, userId);

            if (created == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }

            return RedirectToAction("Details", new { id = created.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            await studyGroupService.JoinGroupAsync(id, userId);

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var group = await studyGroupService.GetByIdAsync(id);

            if (group == null) return NotFound();

            if (group.CreatorId == userId)
            {
                TempData["Error"] = "You cannot leave a group you created.";
                return RedirectToAction("Details", new { id });
            }

            await studyGroupService.LeaveGroupAsync(id, userId);

            return RedirectToAction("Details", new { id });
        }

        
    }
}
