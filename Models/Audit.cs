using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimManagementsystem.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }

        // persisted timestamp
        public DateTime UpdatedOn { get; set; }

        // additional fields used by views
        public int UpdatedBy { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }

        // Optional metadata used by UI; keep persisted columns for Level/Message
        public string Level { get; set; }    // e.g. "Info", "Warning", "Error"
        public string Message { get; set; }  // short message for UI

        // Provide a non-mapped property the views expect (maps to UpdatedOn)
        [NotMapped]
        public DateTime Timestamp => UpdatedOn;
    }

    public class SecurityEvent
    {
        [Key]
        public int SecurityEventId { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public DateTime EventDate { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
    }

    public class SystemLog
    {
        [Key]
        public int SystemLogId { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
    }
}
