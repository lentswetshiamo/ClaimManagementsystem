using ClaimManagementsystem.Models;
using ClaimManagementsystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ClaimService _claimService;

        public ManagerController(ClaimService claimService)
        {
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Manager")
                return RedirectToAction("AccessDenied", "Account");

            var model = new ManagerDashboard
            {
                UserName = HttpContext.Session.GetString("UserName"),
                FinalApprovals = _claimService.GetClaimsForFinalApproval(),
                SystemStats = _claimService.GetSystemStatistics(),
                RecentActivities = _claimService.GetRecentActivities()
            };
            return View(model);
        }

        public IActionResult FinalApproval()
        {
            if (HttpContext.Session.GetString("UserRole") != "Manager")
                return RedirectToAction("AccessDenied", "Account");

            var claims = _claimService.GetClaimsForFinalApproval();
            return View(claims);
        }

        [HttpPost]
        public IActionResult FinalApproveClaim(int claimId, string comments)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            _claimService.ApproveClaim(claimId, userId, comments, "Manager");
            return RedirectToAction("FinalApproval");
        }

        public IActionResult Reports()
        {
            if (HttpContext.Session.GetString("UserRole") != "Manager")
                return RedirectToAction("AccessDenied", "Account");

            var reports = _claimService.GenerateReports();
            return View(reports);
        }
    }
}