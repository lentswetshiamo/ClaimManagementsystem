using ClaimManagementsystem.Models;
using ClaimManagementsystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult AccountPage()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(model.Email, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserName", user.Name ?? "");
                    HttpContext.Session.SetString("UserRole", user.Role ?? "");
                    HttpContext.Session.SetString("UserEmail", user.Email ?? "");

                    // Redirect to the Dashboard action in the controller matching the role (e.g., "Lecturer")
                    return RedirectToAction("Dashboard", user.Role);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("LoginPage", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}