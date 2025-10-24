using Microsoft.AspNetCore.Mvc;


namespace ClaimManagementsystem.Controllers
    {

        public class LecturerController : Controller
        {
            private readonly ClaimService _claimService;

            public LecturerController()
            {
                _claimService = new ClaimService();
            }

            public IActionResult Dashboard()
            {
                if (HttpContext.Session.GetString("UserRole") != "Lecturer")
                    return RedirectToAction("AccessDenied", "Account");

                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var model = new LecturerDashboard
                {
                    UserName = HttpContext.Session.GetString("UserName"),
                    PendingClaims = _claimService.GetClaimsByStatus(userId, "Submitted"),
                    ApprovedClaims = _claimService.GetClaimsByStatus(userId, "Approved"),
                    RejectedClaims = _claimService.GetClaimsByStatus(userId, "Rejected")
                };
                return View(model);
            }

            public IActionResult SubmitClaim()
            {
                if (HttpContext.Session.GetString("UserRole") != "Lecturer")
                    return RedirectToAction("AccessDenied", "Account");

                return View(new ClaimSubmissionModel());
            }

            [HttpPost]
            public IActionResult SubmitClaim(ClaimSubmissionModel model)
            {
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                    var claim = new Claim
                    {
                        UserId = userId,
                        Month = model.Month,
                        Year = model.Year,
                        TotalHours = model.TotalHours,
                        HourlyRate = model.HourlyRate,
                        TotalAmount = model.TotalHours * model.HourlyRate,
                        Status = "Submitted",
                        SubmittedDate = DateTime.Now
                    };

                    _claimService.SubmitClaim(claim);
                    return RedirectToAction("Dashboard");
                }
                return View(model);
            }

            public IActionResult ClaimHistory()
            {
                if (HttpContext.Session.GetString("UserRole") != "Lecturer")
                    return RedirectToAction("AccessDenied", "Account");

                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var claims = _claimService.GetUserClaims(userId);
                return View(claims);
            }
        }
    }