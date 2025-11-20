using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
    public class Audit
    {
        public int Id { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty; // Created, Updated, Verified, Approved, Rejected

        [Required]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Details { get; set; } = string.Empty;

        [StringLength(100)]
        public string IpAddress { get; set; } = string.Empty;
    }
}