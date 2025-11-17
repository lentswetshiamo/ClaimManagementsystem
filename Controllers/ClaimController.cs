using ClaimManagementsystem.Models;
using ClaimManagementsystem.Services;
using ClaimManagementsystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly DocumentService _documentService;
        private readonly DatabaseContext _context;

        public ClaimController(ClaimService claimService, DocumentService documentService, DatabaseContext context)
        {
            _claimService = claimService;
            _documentService = documentService;
            _context = context;
        }

        // Helper method to get current user info from session
        private (int userId, string userName, string userRole) GetCurrentUser()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName") ?? "Guest";
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Lecturer";
            
            int.TryParse(userIdStr, out int userId);
            return (userId, userName, userRole);
        }

        [HttpGet]
        public async Task<IActionResult> Submit()
        {
            var lecturers = await Task.FromResult(_context.Lecturers.ToList());
            ViewBag.Lecturers = lecturers;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim)
        {
            var (userId, userName, userRole) = GetCurrentUser();

            if (!ModelState.IsValid)
            {
                var lecturers = await Task.FromResult(_context.Lecturers.ToList());
                ViewBag.Lecturers = lecturers;
                return View(claim);
            }

            try
            {
                // Validate claim
                if (!_claimService.ValidateClaim(claim, out var errors))
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    var lecturers = await Task.FromResult(_context.Lecturers.ToList());
                    ViewBag.Lecturers = lecturers;
                    return View(claim);
                }

                // Set user information
                claim.UserId = userId;
                claim.SubmittedBy = userName;

                // Get lecturer name if LecturerId is provided
                if (claim.LecturerId > 0)
                {
                    var lecturer = await _context.Lecturers.FindAsync(claim.LecturerId);
                    if (lecturer != null)
                    {
                        claim.LecturerName = lecturer.LecturerName;
                    }
                }

                // Create claim (auto-calculates total, creates audit, workflow, notification)
                await _claimService.CreateClaimAsync(claim, userName);

                TempData["Success"] = "Claim submitted successfully!";
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                var lecturers = await Task.FromResult(_context.Lecturers.ToList());
                ViewBag.Lecturers = lecturers;
                return View(claim);
            }
        }

        [HttpGet]
        public IActionResult UploadDocuments()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocuments(int id, IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                TempData["Error"] = "Please select a file to upload.";
                return View();
            }

            try
            {
                var claim = await _claimService.GetClaimByIdAsync(id);
                if (claim == null)
                {
                    TempData["Error"] = "Claim not found.";
                    return NotFound();
                }

                var documentPath = await _documentService.UploadDocumentAsync(document, id);
                claim.Documents = documentPath;
                
                // Update claim with document path
                await _context.SaveChangesAsync();

                TempData["Success"] = "Document uploaded successfully.";
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error uploading document: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> TrackStatus()
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            IEnumerable<Claim> claims;
            
            if (userRole == "Lecturer")
            {
                // Show lecturer's own claims
                claims = await _claimService.GetClaimsByUserAsync(userId);
            }
            else
            {
                // Show all claims for coordinators and managers
                claims = await _claimService.GetAllClaimsAsync();
            }

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyClaims(int id)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to verify claims.";
                return RedirectToAction("Index", "Dashboard");
            }

            try
            {
                await _claimService.VerifyClaimAsync(id, userName);
                TempData["Success"] = "Claim verified successfully.";
                return RedirectToAction("ViewClaims");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error verifying claim: {ex.Message}";
                return RedirectToAction("ViewClaims");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to approve claims.";
                return RedirectToAction("Index", "Dashboard");
            }

            try
            {
                await _claimService.ApproveClaimAsync(id, userName);
                TempData["Success"] = "Claim approved successfully.";
                return RedirectToAction("ViewClaims");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error approving claim: {ex.Message}";
                return RedirectToAction("ViewClaims");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectClaim(int id)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to reject claims.";
                return RedirectToAction("Index", "Dashboard");
            }

            var claim = await _claimService.GetClaimByIdAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectClaim(int id, string rejectionReason)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                TempData["Error"] = "Rejection reason is required.";
                return RedirectToAction("RejectClaim", new { id });
            }

            try
            {
                await _claimService.RejectClaimAsync(id, userName, rejectionReason);
                TempData["Success"] = "Claim rejected successfully.";
                return RedirectToAction("ViewClaims");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error rejecting claim: {ex.Message}";
                return RedirectToAction("ViewClaims");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewClaims()
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager" && userRole != "HR")
            {
                TempData["Error"] = "You do not have permission to view all claims.";
                return RedirectToAction("Index", "Dashboard");
            }

            IEnumerable<Claim> claims;
            
            if (userRole == "HR")
            {
                // HR sees only approved claims
                claims = await _claimService.GetClaimsByStatusAsync("Approved");
            }
            else
            {
                // Coordinators and Managers see all claims
                claims = await _claimService.GetAllClaimsAsync();
            }

            return View(claims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkApprove(List<int> claimIds)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to approve claims.";
                return RedirectToAction("ViewClaims");
            }

            try
            {
                await _claimService.BulkApproveClaimsAsync(claimIds, userName);
                TempData["Success"] = $"{claimIds.Count} claim(s) approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error approving claims: {ex.Message}";
            }

            return RedirectToAction("ViewClaims");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkReject(List<int> claimIds, string rejectionReason)
        {
            var (userId, userName, userRole) = GetCurrentUser();
            
            if (userRole != "Programme Coordinator" && userRole != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to reject claims.";
                return RedirectToAction("ViewClaims");
            }

            try
            {
                await _claimService.BulkRejectClaimsAsync(claimIds, userName, rejectionReason);
                TempData["Success"] = $"{claimIds.Count} claim(s) rejected successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error rejecting claims: {ex.Message}";
            }

            return RedirectToAction("ViewClaims");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
