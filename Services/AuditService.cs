using ClaimManagementsystem.Data.Repository;
using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class AuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository ?? throw new ArgumentNullException(nameof(auditRepository));
        }

        // Async implementation that persists an audit record using the repository
        public async Task<AuditLog> LogActionAsync(
            string entity,
            int entityId,
            string action,
            string oldValues,
            string newValues,
            int userId,
            string userName,
            string ipAddress,
            string level = "Info",
            string message = null)
        {
            var audit = new AuditLog
            {
                Entity = entity,
                EntityId = entityId,
                Action = action,
                OldValues = oldValues,
                NewValues = newValues,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = userId,
                UserName = userName,
                IpAddress = ipAddress,
                Level = level,
                Message = message ?? action
            };

            return await _auditRepository.AddAsync(audit);
        }

        // Synchronous wrapper for existing callers (keeps compatibility)
        public AuditLog LogAction(
            string entity,
            int entityId,
            string action,
            string oldValues,
            string newValues,
            int userId,
            string userName,
            string ipAddress,
            string level = "Info",
            string message = null)
        {
            return LogActionAsync(entity, entityId, action, oldValues, newValues, userId, userName, ipAddress, level, message)
                .GetAwaiter().GetResult();
        }

        // Async retrieval helpers
        public Task<List<AuditLog>> GetRecentLogsAsync(int count) =>
            _auditRepository.GetRecentAsync(count);

        public Task<List<AuditLog>> GetAllLogsAsync() =>
            _auditRepository.GetByDateRangeAsync(DateTime.MinValue, DateTime.UtcNow);

        public Task<List<AuditLog>> GetByEntityAsync(string entity, int entityId) =>
            _auditRepository.GetByEntityAsync(entity, entityId);

        public Task<List<AuditLog>> GetByUserAsync(int userId) =>
            _auditRepository.GetByUserAsync(userId);

        // Synchronous wrappers
        public List<AuditLog> GetRecentLogs(int count) =>
            GetRecentLogsAsync(count).GetAwaiter().GetResult();

        public List<AuditLog> GetAllLogs() =>
            GetAllLogsAsync().GetAwaiter().GetResult();

        public List<AuditLog> GetAuditTrailForClaim(int claimId) =>
            GetByEntityAsync("Claim", claimId).GetAwaiter().GetResult();
    }
}