using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ClaimManagementsystem.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public int ClaimId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadedBy { get; set; }
        public string Description { get; set; }
    }

    public class DocumentUploadModel
    {
        public int ClaimId { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

    public class DocumentValidationResult
    {
        public bool IsValid { get; set; }
        public string[] Errors { get; set; }
        public string[] Warnings { get; set; }
    }
}