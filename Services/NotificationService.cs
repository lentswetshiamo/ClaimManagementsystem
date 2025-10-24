using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class NotificationService
    {
        public readonly List<Notification> _notifications;
        public readonly List<EmailTemplate> _emailTemplates;

            public NotificationService()
            {
                _notifications = new List<Notification>();
                _emailTemplates = InitializeEmailTemplates();
            }

                public List<EmailTemplate> InitializeEmailTemplates()
            {
                return new List<EmailTemplate>
            {
                new EmailTemplate {
                    TemplateId = 1,
                    TemplateName = "ClaimSubmitted",
                    Subject = "Claim Submission Confirmation - CMCS",
                    Body = "Dear {UserName},<br><br>Your claim for {Month} {Year} has been successfully submitted.<br>Claim Reference: {ClaimId}<br>Total Amount: {TotalAmount}<br><br>Thank you,<br>CMCS Team",
                    IsActive = true
                },
                new EmailTemplate {
                    TemplateId = 2,
                    TemplateName = "ClaimApproved",
                    Subject = "Claim Approved - CMCS",
                    Body = "Dear {UserName},<br><br>Your claim for {Month} {Year} has been approved.<br>Claim Reference: {ClaimId}<br>Approved Amount: {TotalAmount}<br><br>Thank you,<br>CMCS Team",
                    IsActive = true
                },
                new EmailTemplate {
                    TemplateId = 3,
                    TemplateName = "ClaimRejected",
                    Subject = "Claim Requires Attention - CMCS",
                    Body = "Dear {UserName},<br><br>Your claim for {Month} {Year} requires additional information.<br>Claim Reference: {ClaimId}<br>Comments: {Comments}<br><br>Please review and resubmit.<br><br>Thank you,<br>CMCS Team",
                    IsActive = true
                }
            };
            }

            public void SendClaimSubmittedNotification(int userId, Claim claim)
            {
                var template = _emailTemplates.First(t => t.TemplateName == "ClaimSubmitted");
                var message = template.Body
                    .Replace("{UserName}", claim.UserName)
                    .Replace("{Month}", claim.Month)
                    .Replace("{Year}", claim.Year.ToString())
                    .Replace("{ClaimId}", claim.ClaimId.ToString())
                    .Replace("{TotalAmount}", claim.TotalAmount.ToString("C"));

                AddNotification(userId, "Claim Submitted", message, "info", $"/Lecturer/ClaimDetails/{claim.ClaimId}");
            }

            public void SendClaimApprovedNotification(int userId, Claim claim)
            {
                var template = _emailTemplates.First(t => t.TemplateName == "ClaimApproved");
                var message = template.Body
                    .Replace("{UserName}", claim.UserName)
                    .Replace("{Month}", claim.Month)
                    .Replace("{Year}", claim.Year.ToString())
                    .Replace("{ClaimId}", claim.ClaimId.ToString())
                    .Replace("{TotalAmount}", claim.TotalAmount.ToString("C"));

                AddNotification(userId, "Claim Approved", message, "success", $"/Lecturer/ClaimDetails/{claim.ClaimId}");
            }

            public void SendClaimRejectedNotification(int userId, Claim claim, string comments)
            {
                var template = _emailTemplates.First(t => t.TemplateName == "ClaimRejected");
                var message = template.Body
                    .Replace("{UserName}", claim.UserName)
                    .Replace("{Month}", claim.Month)
                    .Replace("{Year}", claim.Year.ToString())
                    .Replace("{ClaimId}", claim.ClaimId.ToString())
                    .Replace("{Comments}", comments);

                AddNotification(userId, "Claim Requires Attention", message, "warning", $"/Lecturer/ClaimDetails/{claim.ClaimId}");
            }

            public void SendNewClaimNotificationToCoordinators(Claim claim)
            {
                var coordinators = new List<int> { 2 }; // Sample coordinator IDs
                foreach (var coordinatorId in coordinators)
                {
                    AddNotification(coordinatorId, "New Claim for Review",
                        $"New claim submitted by {claim.UserName} for {claim.Month} {claim.Year}",
                        "info", $"/Coordinator/ClaimReview/{claim.ClaimId}");
                }
            }

            public void AddNotification(int userId, string title, string message, string type, string actionUrl)
            {
                _notifications.Add(new Notification
                {
                    NotificationId = _notifications.Count + 1,
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = type,
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    ActionUrl = actionUrl
                });
            }

            public List<Notification> GetUserNotifications(int userId)
            {
                return _notifications.Where(n => n.UserId == userId && !n.IsRead)
                                   .OrderByDescending(n => n.CreatedDate)
                                   .ToList();
            }

            public void MarkAsRead(int notificationId)
            {
                var notification = _notifications.FirstOrDefault(n => n.NotificationId == notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                }
            }
        }
    }

