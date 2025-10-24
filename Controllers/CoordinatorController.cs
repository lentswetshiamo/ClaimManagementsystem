using ClaimManagementsystem.Models;
using ClaimManagementsystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ClaimService _claimService;

        public CoordinatorController(ClaimService claimService)
        {
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Coordinator")
                return RedirectToAction("AccessDenied", "Account");

            var model = new CoordinatorDashboard
            {
                UserName = HttpContext.Session.GetString("UserName"),
                PendingReviews = _claimService.GetClaimsForReview(),
                RecentApprovals = _claimService.GetRecentApprovals()
            };
            return View(model);
        }

        public IActionResult ReviewClaims()
        {
            if (HttpContext.Session.GetString("UserRole") != "Coordinator")
                return RedirectToAction("AccessDenied", "Account");

            var claims = _claimService.GetClaimsForReview();
            return View(claims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int claimId, string comments)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            _claimService.ApproveClaim(claimId, userId, comments, "Coordinator");
            return RedirectToAction("ReviewClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int claimId, string comments)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            _claimService.RejectClaim(claimId, userId, comments);
            return RedirectToAction("ReviewClaims");
        }
    }
}