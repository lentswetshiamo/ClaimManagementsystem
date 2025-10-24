using System.Reflection.Metadata;
using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class DocumentService
    {
        public readonly List<Document> _documents;
        public readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB
        public readonly string[] _allowedExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };

            public DocumentService()
            {
                _documents = new List<Document>();
            }

            public DocumentValidationResult ValidateDocument(IFormFile file)
            {
                var result = new DocumentValidationResult { IsValid = true, Errors = new string[0], Warnings = new string[0] };
                var errors = new List<string>();
                var warnings = new List<string>();

                // Check file size
                if (file.Length > _maxFileSize)
                {
                    errors.Add($"File size exceeds maximum allowed size of {_maxFileSize / 1024 / 1024}MB");
                }

                // Check file extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    errors.Add($"File type not allowed. Allowed types: {string.Join(", ", _allowedExtensions)}");
                }

                // Check for potential security issues
                if (file.FileName.Contains("..") || file.FileName.Contains("/") || file.FileName.Contains("\\"))
                {
                    errors.Add("Invalid file name");
                }

                result.IsValid = !errors.Any();
                result.Errors = errors.ToArray();
                result.Warnings = warnings.ToArray();

                return result;
            }

            public async Task<Document> SaveDocumentAsync(IFormFile file, int claimId, int userId, string description)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new Document
                {
                    DocumentId = _documents.Count + 1,
                    ClaimId = claimId,
                    FileName = file.FileName,
                    FilePath = $"/uploads/{uniqueFileName}",
                    FileType = Path.GetExtension(file.FileName),
                    FileSize = file.Length,
                    UploadDate = DateTime.Now,
                    UploadedBy = userId,
                    Description = description
                };

                _documents.Add(document);
                return document;
            }

            public List<Document> GetClaimDocuments(int claimId)
            {
                return _documents.Where(d => d.ClaimId == claimId).ToList();
            }

            public Document GetDocument(int documentId)
            {
                return _documents.FirstOrDefault(d => d.DocumentId == documentId);
            }

            public bool DeleteDocument(int documentId)
            {
                var document = _documents.FirstOrDefault(d => d.DocumentId == documentId);
                if (document != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.FilePath.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    _documents.Remove(document);
                    return true;
                }
                return false;
            }
        }
}
