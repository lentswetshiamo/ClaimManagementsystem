namespace ClaimManagementsystem.Services
{
    public class DocumentService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(IWebHostEnvironment environment, ILogger<DocumentService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> UploadDocumentAsync(IFormFile file, int claimId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid file type. Allowed types: PDF, DOC, DOCX, TXT, JPG, PNG");
            }

            // Validate file size (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("File size exceeds 5MB limit");
            }

            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "claims");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var uniqueFileName = $"{claimId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            _logger.LogInformation($"Document uploaded successfully: {uniqueFileName}");
            return $"/uploads/claims/{uniqueFileName}";
        }

        public bool DeleteDocument(string documentPath)
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, documentPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation($"Document deleted successfully: {documentPath}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting document: {documentPath}");
                return false;
            }
        }

        public bool DocumentExists(string documentPath)
        {
            var filePath = Path.Combine(_environment.WebRootPath, documentPath.TrimStart('/'));
            return File.Exists(filePath);
        }
    }
}