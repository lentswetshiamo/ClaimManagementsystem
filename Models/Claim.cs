using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
        public class Claim
        {
            [Key]
            public int ClaimId { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
            public decimal TotalHours { get; set; }
            public decimal HourlyRate { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public DateTime SubmittedDate { get; set; }
            public DateTime? ApprovedDate { get; set; }
            public string Comments { get; set; }

            // Navigation property
            public virtual User User { get; set; }
        }

        public class ClaimSubmissionModel
        {
            public string Month { get; set; }
            public int Year { get; set; }
            public decimal TotalHours { get; set; }
            public decimal HourlyRate { get; set; }
            public List<ClaimItem> ClaimItems { get; set; } = new List<ClaimItem>();
        }

        public class ClaimItem
        {
            [Key]
            public int ClaimItemId { get; set; }
            public int ClaimId { get; set; }
            public string Module { get; set; }
            public decimal HoursWorked { get; set; }
            public decimal Rate { get; set; }
            public decimal Amount => HoursWorked * Rate;
        }

        public class SystemStatistics
        {
            public int TotalClaims { get; set; }
            public int PendingClaims { get; set; }
            public int ApprovedClaims { get; set; }
            public decimal TotalAmount { get; set; }
            public int TotalUsers { get; set; }
            public int ActiveSessions { get; set; }
            public string SystemUptime { get; set; }
            public string StorageUsed { get; set; }
        }

        public class Activity
        {
            public string Description { get; set; }
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
        }
    }

    