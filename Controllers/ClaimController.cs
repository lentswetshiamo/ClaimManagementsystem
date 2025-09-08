using ClaimManagementsystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class ClaimController : Controller
    {
        public static List<Claim> claims = new List<Claim>();
        public static List<User> users = new List<User>();
        public static List<Lecturer> lecturers = new List<Lecturer>();
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            ViewBag.Users = users;
            ViewBag.Lecturers = lecturers;
            return View();
        }
        [HttpPost]
        public IActionResult Submit(Claim claim, Lecturer lecturer)
        {
            claim.Id = claims.Count + 1;
            //Use lecturer model to get lecturer details
            claim.LecturerName = lecturer.LecturerName ?? TempData["UserName"]?.ToString();
            claim.Status = "Pending";
            claim.DateSubmitted = DateTime.Now;
            claims.Add(claim);
            TempData["Message"] = "Claim submitted successfully.";
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return RedirectToAction("Index", "Dashboard");

        }
        [HttpGet]
        public IActionResult UploadDocuments()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadDocuments(int id, string documentPath)
        {
            var claim = claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Documents = documentPath;
                TempData["Message"] = "Document uploaded successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            return NotFound();

        }
        [HttpGet]
        public IActionResult TrackStatus()
        {
            var lecturerName = TempData["UserName"]?.ToString();
            var lecturerClaims = claims.Where(c => c.LecturerName == lecturerName).ToList();
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View(lecturerClaims);
        }

        [HttpGet]
        public IActionResult VerifyClaims(int id)
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to verify claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            var claim = claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Verified";
                TempData["Message"] = "Claim verified successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ApproveClaim(int id)
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to approve claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            var claim = claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Approved";
                TempData["Message"] = "Claim approved successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ViewClaim()
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to view claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View(claims);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
