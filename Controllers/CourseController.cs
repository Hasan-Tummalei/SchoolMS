using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SchoolManagementSystem.Services;

namespace CourseManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseController(ICourseService courseService, IHttpContextAccessor httpContextAccessor)
        {
            _courseService = courseService;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        // GET: /Course
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: /Course/Create
        [Authorize(Roles = "Teacher")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        [Authorize(Roles = "Teacher")]

        [HttpPost]
        public async Task<IActionResult> Create(string title, string description)
        {
            //if (Session.GetString("UserRole") != "Teacher")
            //{
            //    return Unauthorized();
            //}

            var teacherId = int.Parse(Session.GetString("UserId"));

            if (!await _courseService.CreateCourseAsync(title, description, teacherId))
            {
                ModelState.AddModelError("", "Unable to create course.");
                return View();
            }

            return RedirectToAction("Index");
        }


        // GET: /Course/Edit/{id}
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Edit/{id}
        [Authorize(Roles = "Teacher")]

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string title, string description)
        {
            var teacherId = int.Parse(Session.GetString("UserId"));

            if (!await _courseService.UpdateCourseAsync(id, title, description, teacherId))
            {
                ModelState.AddModelError("", "Unable to update course.");
                return View();
            }

            return RedirectToAction("Index");
        }

        // POST: /Course/Delete/{id}
        [Authorize(Roles = "Teacher")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var teacherId = int.Parse(Session.GetString("UserId"));

            if (!await _courseService.DeleteCourseAsync(id, teacherId))
            {
                return Unauthorized();
            }

            return RedirectToAction("Index");
        }
    }
}