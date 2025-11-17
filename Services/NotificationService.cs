using ClaimManagementsystem.Data;
using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Services
{
    public class NotificationService
    {
        private readonly DatabaseContext _context;

        public NotificationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task NotifyClaimStatusChangeAsync(int claimId, int userId, string status)
        {
            var notification = new Notification
            {
                ClaimId = claimId,
                UserId = userId,
                Title = $"Claim Status Update",
                Message = $"Your claim has been {status}.",
                Type = status == "approved" ? "Success" : status == "rejected" ? "Error" : "Info",
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
