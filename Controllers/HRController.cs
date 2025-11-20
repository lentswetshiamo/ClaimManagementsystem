using ClaimManagementsystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ClaimManagementsystem.Controllers
{
    public class HRController : Controller
    {
        private readonly ClaimService _claimService;

        public HRController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        private (int userId, string userName, string userRole) GetCurrentUser()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName") ?? "Guest";
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Lecturer";

            int.TryParse(userIdStr, out int userId);
            return (userId, userName, userRole);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (userId, userName, userRole) = GetCurrentUser();

            if (userRole != "HR")
            {
                TempData["Error"] = "You do not have permission to access HR functions.";
                return RedirectToAction("Index", "Dashboard");
            }

            // Get approved claims for HR
            var approvedClaims = await _claimService.GetClaimsByStatusAsync("Approved");
            return View(approvedClaims);
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyReport(int? year, int? month)
        {
            var (userId, userName, userRole) = GetCurrentUser();

            if (userRole != "HR")
            {
                TempData["Error"] = "You do not have permission to access HR functions.";
                return RedirectToAction("Index", "Dashboard");
            }

            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            ViewBag.SelectedYear = selectedYear;
            ViewBag.SelectedMonth = selectedMonth;

            var allClaims = await _claimService.GetClaimsByStatusAsync("Approved");
            var monthlyClaims = allClaims.Where(c =>
                c.DateSubmitted.Year == selectedYear &&
                c.DateSubmitted.Month == selectedMonth).ToList();

            return View(monthlyClaims);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCsv(int? year, int? month)
        {
            var (userId, userName, userRole) = GetCurrentUser();

            if (userRole != "HR")
            {
                TempData["Error"] = "You do not have permission to export reports.";
                return RedirectToAction("Index", "Dashboard");
            }

            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            var allClaims = await _claimService.GetClaimsByStatusAsync("Approved");
            var monthlyClaims = allClaims.Where(c =>
                c.DateSubmitted.Year == selectedYear &&
                c.DateSubmitted.Month == selectedMonth).ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Claim ID,Claim Type,Lecturer Name,Hours Worked,Hourly Rate,Total Amount,Date Submitted,Approved By,Approved Date");

            foreach (var claim in monthlyClaims)
            {
                csv.AppendLine($"{claim.Id},{claim.ClaimType},{claim.LecturerName},{claim.HoursWorked},{claim.HourlyRate},{claim.TotalAmount},{claim.DateSubmitted:yyyy-MM-dd},{claim.ApprovedBy},{claim.ApprovedDate:yyyy-MM-dd}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"Claims_Report_{selectedYear}_{selectedMonth:00}.csv";

            return File(bytes, "text/csv", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            var (userId, userName, userRole) = GetCurrentUser();

            if (userRole != "HR")
            {
                TempData["Error"] = "You do not have permission to access HR functions.";
                return RedirectToAction("Index", "Dashboard");
            }

            var allClaims = await _claimService.GetAllClaimsAsync();

            ViewBag.TotalClaims = allClaims.Count();
            ViewBag.ApprovedClaims = allClaims.Count(c => c.Status == "Approved");
            ViewBag.PendingClaims = allClaims.Count(c => c.Status == "Pending");
            ViewBag.RejectedClaims = allClaims.Count(c => c.Status == "Rejected");
            ViewBag.VerifiedClaims = allClaims.Count(c => c.Status == "Verified");
            ViewBag.TotalAmount = allClaims.Where(c => c.Status == "Approved").Sum(c => c.TotalAmount);

            return View();
        }
    }
}