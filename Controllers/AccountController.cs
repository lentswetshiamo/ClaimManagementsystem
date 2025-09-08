using ClaimManagementsystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class AccountController : Controller
    {
        public static List<User> users = new List<User>();
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            user.Id = users.Count + 1;
            users.Add(user);
            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Corrected the variable name to reference the 'users' list
            var user = users.Find(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                TempData["UserName"] = user.Name;
                TempData["UserRole"] = user.Role;
                return RedirectToAction("Index", "Dashboard");
            }

            // If user is not found, still allow login with dummy redirect
            TempData["UserName"] = "Demo User";
            TempData["UserRole"] = "Lecturer";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login");
        }
    }
}
