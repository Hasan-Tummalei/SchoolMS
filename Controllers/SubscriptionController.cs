using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Services;

namespace CourseManagement.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionController(ISubscriptionService subscriptionService, IHttpContextAccessor httpContextAccessor)
        {
            _subscriptionService = subscriptionService;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        // GET: /Subscription
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> Index()
        {
            if (Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = int.Parse(Session.GetString("UserId"));

            var subscriptions = await _subscriptionService.GetSubscriptionsByStudentAsync(studentId);
            return View(subscriptions);
        }

        // POST: /Subscription/Subscribe/{courseId}
        [Authorize(Roles = "Student")]

        [HttpPost]
        public async Task<IActionResult> Subscribe(int courseId)
        {
            if (Session.GetString("UserRole") != "Student")
            {
                return Unauthorized();
            }

            var studentId = int.Parse(Session.GetString("UserId"));

            if (!await _subscriptionService.SubscribeAsync(studentId, courseId))
            {
                return BadRequest("Already subscribed.");
            }

            return RedirectToAction("Index");
        }

        // POST: /Subscription/Unsubscribe/{courseId}
        [Authorize(Roles = "Student")]

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int courseId)
        {
            if (Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = int.Parse(Session.GetString("UserId"));

            if (!await _subscriptionService.UnsubscribeAsync(studentId, courseId))
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}