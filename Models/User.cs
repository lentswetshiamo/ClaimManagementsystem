using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ClaimManagementsystem.Models
{
        public class User
        {
            [Key]
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public decimal HourlyRate { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastLogin { get; set; }
        public object Id { get; internal set; }
    }

        public class LoginModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class LecturerDashboard
        {
            public string UserName { get; set; }
            public List<Claim> PendingClaims { get; set; }
            public List<Claim> ApprovedClaims { get; set; }
            public List<Claim> RejectedClaims { get; set; }
        }

        public class CoordinatorDashboard
        {
            public string UserName { get; set; }
            public List<Claim> PendingReviews { get; set; }
            public List<Claim> RecentApprovals { get; set; }
        }

        public class ManagerDashboard
        {
            public string UserName { get; set; }
            public List<Claim> FinalApprovals { get; set; }
            public SystemStatistics SystemStats { get; set; }
            public List<Activity> RecentActivities { get; set; }
        }

        public class AdminDashboard
        {
            public string UserName { get; set; }
            public SystemStatistics SystemStats { get; set; }
            public List<AuditLog> RecentLogs { get; set; }
        }
    }