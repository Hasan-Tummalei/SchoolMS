using System.Security.Claims;
using CourseManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Services;

namespace CourseManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        // GET: /Account/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the homepage or dashboard
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, UserRole role)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the homepage or dashboard
            }
            if (!await _userService.RegisterAsync(name, email, password, role))
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View();
            }

            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the homepage or dashboard
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {


            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Prevent re-login
            }
            var user = await _userService.LoginAsync(email, password);

            if (user == null)
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View();
            }

            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()) // Add role claim
    };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            await HttpContext.SignInAsync("CookieAuth", principal);

            // Store user data in session
            Session.SetString("UserId", user.Id.ToString());
            Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("Index", "Home");
        }

        // User logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}