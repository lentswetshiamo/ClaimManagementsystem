using ClaimManagementsystem.Models;

namespace CaimManagementSystem.Models
    {
        public class AdminDashboard
        {
            public string UserName { get; set; }
            public SystemStatistics SystemStats { get; set; }
            public List<AuditLog> RecentLogs { get; set; }
        }

        public class SystemStatistics
        {
            public int TotalUsers { get; set; }
            public int ActiveSessions { get; set; }
            public string SystemUptime { get; set; }
            public string StorageUsed { get; set; }
            public int TotalClaims { get; set; }
            public int PendingClaims { get; set; }
            public int ApprovedClaims { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public class Activity
        {
            public string Description { get; set; }
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
        }
    }