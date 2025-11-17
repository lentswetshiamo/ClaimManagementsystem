using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
    public class Workflow
    {
        public int Id { get; set; }
        
        [Required]
        public int ClaimId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CurrentStage { get; set; } = string.Empty; // Submitted, UnderReview, Verified, Approved, Rejected
        
        [StringLength(100)]
        public string? AssignedTo { get; set; }
        
        [Required]
        public DateTime StageStartDate { get; set; } = DateTime.Now;
        
        public DateTime? StageEndDate { get; set; }
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        public bool RequiresAction { get; set; } = true;
        
        [StringLength(100)]
        public string? NextApprover { get; set; }
    }
}
