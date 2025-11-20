using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimManagementsystem.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Claim type is required")]
        [StringLength(100, ErrorMessage = "Claim type cannot exceed 100 characters")]
        public string ClaimType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Verified, Approved, Rejected

        [StringLength(500)]
        public string Documents { get; set; } = string.Empty;

        [Range(0, 1000, ErrorMessage = "Hours worked must be between 0 and 1000")]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Range(0, 10000, ErrorMessage = "Hourly rate must be between 0 and 10000")]
        [Display(Name = "Hourly Rate")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; }

        // Auto-calculated field
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; } = string.Empty;

        [Required]
        public int LecturerId { get; set; }

        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; } = string.Empty;

        // Approval tracking
        [StringLength(100)]
        public string? ApprovedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(500)]
        public string? RejectionReason { get; set; }

        // Method to calculate total
        public void CalculateTotal()
        {
            TotalAmount = HoursWorked * HourlyRate;
        }

        // Validation method
        public bool IsValid(out List<string> errors)
        {
            errors = new List<string>();

            if (HoursWorked <= 0)
                errors.Add("Hours worked must be greater than zero");

            if (HourlyRate <= 0)
                errors.Add("Hourly rate must be greater than zero");

            if (string.IsNullOrWhiteSpace(ClaimType))
                errors.Add("Claim type is required");

            if (string.IsNullOrWhiteSpace(Description) || Description.Length < 10)
                errors.Add("Description must be at least 10 characters");

            return errors.Count == 0;
        }
    }
}