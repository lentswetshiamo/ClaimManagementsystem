using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
    public class Notification
        {
            [Key]
            public int NotificationId { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string Type { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ActionUrl { get; set; }
        }

        public class EmailTemplate
        {
            [Key]
            public int EmailTemplateId { get; set; }
            public string TemplateName { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool IsActive { get; set; }
        }

        public class NotificationPreference
        {
            [Key]
            public int NotificationPreferenceId { get; set; }
            public int UserId { get; set; }
            public bool EmailNotifications { get; set; }
            public bool PushNotifications { get; set; }
            public bool ClaimSubmitted { get; set; }
            public bool ClaimApproved { get; set; }
            public bool ClaimRejected { get; set; }
            public bool SystemUpdates { get; set; }
        }
    }