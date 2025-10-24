using ClaimManagementsystem.Models;
using ClaimManagementsystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthService _authService;
        private readonly AuditService _auditService;

        public AdminController(AuthService authService, AuditService auditService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            var model = new AdminDashboard
            {
                UserName = HttpContext.Session.GetString("UserName"),
                SystemStats = new SystemStatistics
                {
                    TotalUsers = _authService.GetAllUsers().Count,
                    ActiveSessions = 15,
                    SystemUptime = "99.9%",
                    StorageUsed = "2.3GB"
                },
                RecentLogs = await _auditService.GetRecentLogsAsync(10)
            };
            return View(model);
        }

        public IActionResult UserManagement()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            var users = _authService.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> SystemLogs()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            var logs = await _auditService.GetAllLogsAsync();
            return View(logs);
        }
    }
}
