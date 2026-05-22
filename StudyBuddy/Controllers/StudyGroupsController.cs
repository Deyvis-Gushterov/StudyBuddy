using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Models;
using StudyBuddy.Services.Implementations;
using StudyBuddy.Services.Interfaces;
using StudyBuddy.ViewModels;
using System.Security.Claims;

namespace StudyBuddy.Controllers
{
    [Authorize]
    public class StudyGroupsController: Controller
    {
        private readonly IStudyGroupService studyGroupService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INoteService noteService;
        private readonly IBlogService blogService;

        public StudyGroupsController(IStudyGroupService studygroupService,
                                          UserManager<ApplicationUser> userManager,
                                          INoteService noteService,
                                          IBlogService blogService)
        {
            this.studyGroupService = studygroupService;
            this.userManager = userManager;
            this.noteService = noteService;
            this.blogService = blogService;
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

            var allNotes = await noteService.GetAllNotesAsync();
            var allBlogs = await blogService.GetAllBlogsAsync();

            var viewModel = new StudyGroupDetailsViewModel
            {
                Group = group,
                IsMember = await studyGroupService.IsUserMemberAsync(group.Id, userId!),
                IsCreator = group.CreatorId == userId,
                UserNotes = allNotes.Where(n => n.CreatorId == userId).ToList(),
                UserBlogs = allBlogs.Where(b => b.AuthorId == userId).ToList(),
                CurrentUserId = userId!
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

        [HttpPost]
        public async Task<IActionResult> AddNote(int id, int noteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await studyGroupService.AddNoteAsync(id, noteId, userId);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveNote(int id, int noteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await studyGroupService.RemoveNoteAsync(id, noteId, userId);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddBlog(int id, int blogId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await studyGroupService.AddBlogAsync(id, blogId, userId);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBlog(int id, int blogId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await studyGroupService.RemoveBlogAsync(id, blogId, userId);
            return RedirectToAction("Details", new { id });
        }
    }
}
