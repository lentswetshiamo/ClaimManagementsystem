using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Data.Repository
{
    public interface IAuditRepository
        {
            Task<AuditLog> AddAsync(AuditLog auditLog);
            Task<List<AuditLog>> GetByEntityAsync(string entity, int entityId);
            Task<List<AuditLog>> GetByUserAsync(int userId);
            Task<List<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
            Task<List<AuditLog>> GetRecentAsync(int count);
        }
    }

