using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClaimManagementsystem.Data;
using ClaimManagementsystem.Data.Repository;
using ClaimManagementsystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly IClaimRepository _claimRepository;
        private readonly DatabaseContext _db;

        private const long MaxFileBytes = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".png", ".jpg", ".jpeg" };

        public ClaimController(IClaimRepository claimRepository, DatabaseContext db)
        {
            _claimRepository = claimRepository ?? throw new ArgumentNullException(nameof(claimRepository));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Submit()
        {
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View();
        }

        // Lecturers submit a claim. Supports optional single file upload at submission time.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim, IFormFile upload = null)
        {
            try
            {
                if (claim == null)
                {
                    TempData["Error"] = "Invalid claim data.";
                    return RedirectToAction("Submit");
                }

                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Please correct the highlighted errors and try again.";
                    return View(claim);
                }

                claim.TotalAmount = claim.TotalHours * claim.HourlyRate;
                claim.SubmittedDate = DateTime.UtcNow;
                claim.Status = "Pending";
                if (string.IsNullOrWhiteSpace(claim.UserName))
                {
                    claim.UserName = TempData["UserName"]?.ToString() ?? claim.UserName;
                }

                // persist claim first so we have an id for files
                var added = await _claimRepository.AddAsync(claim as Claim);

                // handle optional upload   
                if (upload != null && upload.Length > 0)
                {
                    var saveResult = await SaveFileForClaimAsync(added.ClaimId, upload, UserIdFromTempData());
                    if (!saveResult.success)
                    {
                        TempData["Error"] = saveResult.errorMessage;
                        TempData.Keep("UserName");
                        TempData.Keep("UserRole");
                        return RedirectToAction("Submit");
                    }
                }

                TempData["Message"] = "Claim submitted successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while submitting the claim: " + ex.Message;
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return View(claim);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UploadDocuments(int id)
        {
            var claim = await _claimRepository.GetByIdAsync(id);
            if (claim == null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction("Index", "Dashboard");
            }

            var files = await _db.Documents.Where(d => d.ClaimId == id).Select(d => d.FileName).ToListAsync();
            ViewBag.Claim = claim;
            ViewBag.UploadedFiles = files;
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View();
        }

        // Upload document(s) for an existing claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocuments(int id, IFormFile file, string description = null)
        {
            try
            {
                var claim = await _claimRepository.GetByIdAsync(id);
                if (claim == null)
                {
                    TempData["Error"] = "Claim not found.";
                    return RedirectToAction("Index", "Dashboard");
                }

                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "No file selected.";
                    return RedirectToAction(nameof(UploadDocuments), new { id });
                }

                var saveResult = await SaveFileForClaimAsync(id, file, UserIdFromTempData(), description);
                if (!saveResult.success)
                {
                    TempData["Error"] = saveResult.errorMessage;
                    TempData.Keep("UserName");
                    TempData.Keep("UserRole");
                    return RedirectToAction(nameof(UploadDocuments), new { id });
                }

                TempData["Message"] = "Document uploaded successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while uploading the document: " + ex.Message;
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction(nameof(UploadDocuments), new { id });
            }
        }

        // Tracking for lecturers: show claims for current lecturer
        [HttpGet]
        public async Task<IActionResult> TrackStatus()
        {
            try
            {
                var lecturerName = TempData["UserName"]?.ToString();
                if (string.IsNullOrEmpty(lecturerName))
                {
                    TempData["Error"] = "User not identified. Please log in.";
                    return RedirectToAction("Index", "Dashboard");
                }

                var lecturerClaims = await _db.Claims
                    .Where(c => c.UserName == lecturerName)
                    .OrderByDescending(c => c.SubmittedDate)
                    .ToListAsync();

                ViewBag.UploadedFiles = await _db.Documents.GroupBy(d => d.ClaimId).ToDictionaryAsync(g => g.Key, g => g.Select(x => x.FileName).ToList());
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return View(lecturerClaims);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Unable to load claim status: " + ex.Message;
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }
        }

        // View that a coordinator/manager uses to review pending claims
        [HttpGet]
        public async Task<IActionResult> VerifyClaims()
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to verify claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }

            var pendingClaims = await _claimRepository.GetByStatusAsync("Pending");
            ViewBag.UploadedFiles = await _db.Documents.GroupBy(d => d.ClaimId).ToDictionaryAsync(g => g.Key, g => g.Select(x => x.FileName).ToList());
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View(pendingClaims);
        }

        // Approve a claim (POST to change state)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to approve claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }

            var claim = await _claimRepository.GetByIdAsync(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.ApprovedDate = DateTime.UtcNow;
                await _claimRepository.UpdateAsync(claim);

                TempData["Message"] = $"Claim {id} approved successfully.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction(nameof(VerifyClaims));
            }

            TempData["Error"] = "Claim not found.";
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return RedirectToAction(nameof(VerifyClaims));
        }

        // Reject a claim (POST to change state)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectClaim(int id, string rejectionReason)
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager")
            {
                TempData["Error"] = "You do not have permission to reject claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }

            var claim = await _claimRepository.GetByIdAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovedDate = DateTime.UtcNow;
                claim.Comments = string.IsNullOrWhiteSpace(rejectionReason) ? "Rejected by verifier." : rejectionReason;
                await _claimRepository.UpdateAsync(claim);

                TempData["Message"] = $"Claim {id} rejected.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction(nameof(VerifyClaims));
            }

            TempData["Error"] = "Claim not found.";
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return RedirectToAction(nameof(VerifyClaims));
        }

        // View all claims (admin/coordinator/manager)
        [HttpGet]
        public async Task<IActionResult> ViewAllClaims()
        {
            var role = TempData["UserRole"]?.ToString();
            if (role != "Programme Coordinator" && role != "Academic Manager" && role != "Admin")
            {
                TempData["Error"] = "You do not have permission to view all claims.";
                TempData.Keep("UserName");
                TempData.Keep("UserRole");
                return RedirectToAction("Index", "Dashboard");
            }

            var allClaims = await _db.Claims.OrderByDescending(c => c.SubmittedDate).ToListAsync();
            ViewBag.UploadedFiles = await _db.Documents.GroupBy(d => d.ClaimId).ToDictionaryAsync(g => g.Key, g => g.Select(x => x.FileName).ToList());
            TempData.Keep("UserName");
            TempData.Keep("UserRole");
            return View(allClaims);
        }

        public IActionResult Index()
        {
            return View();
        }

        // Helper: save uploaded file to disk and create Document record
        private async Task<(bool success, string errorMessage)> SaveFileForClaimAsync(int claimId, IFormFile file, int uploadedBy, string description = null)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "File is empty.");
            }

            if (file.Length > MaxFileBytes)
            {
                return (false, $"File exceeds maximum allowed size of {MaxFileBytes / (1024 * 1024)} MB.");
            }

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext.ToLowerInvariant()))
            {
                return (false, "File type is not allowed. Allowed types: PDF, DOCX, XLSX, PNG, JPG.");
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a safe unique filename
                var safeFileName = $"{claimId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, safeFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create document record
                var doc = new Document
                {
                    ClaimId = claimId,
                    FileName = safeFileName,
                    FilePath = "/uploads/" + safeFileName,
                    FileType = ext,
                    FileSize = file.Length,
                    UploadDate = DateTime.UtcNow,
                    UploadedBy = uploadedBy,
                    Description = description
                };

                _db.Documents.Add(doc);
                await _db.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Failed to save file: " + ex.Message);
            }
        }

        private int UserIdFromTempData()
        {
            if (int.TryParse(TempData["UserId"]?.ToString(), out var id))
            {
                return id;
            }
            return 0;
        }
    }
}
