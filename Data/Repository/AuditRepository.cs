using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Data.Repository
{
    public class AuditRepository : IAuditRepository
        {
            private readonly DatabaseContext _context;

            public AuditRepository(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<AuditLog> AddAsync(AuditLog auditLog)
            {
                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();
                return auditLog;
            }

            public async Task<List<AuditLog>> GetByEntityAsync(string entity, int entityId)
            {
                return await _context.AuditLogs
                    .Where(a => a.Entity == entity && a.EntityId == entityId)
                    .OrderByDescending(a => a.UpdatedOn)
                    .ToListAsync();
            }

            public async Task<List<AuditLog>> GetByUserAsync(int userId)
            {
                return await _context.AuditLogs
                    .Where(a => a.UpdatedBy == userId)
                    .OrderByDescending(a => a.UpdatedOn)
                    .ToListAsync();
            }

            public async Task<List<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
            {
                return await _context.AuditLogs
                    .Where(a => a.UpdatedOn >= startDate && a.UpdatedOn <= endDate)
                    .OrderByDescending(a => a.UpdatedOn)
                    .ToListAsync();
            }

            public async Task<List<AuditLog>> GetRecentAsync(int count)
            {
                return await _context.AuditLogs
                    .OrderByDescending(a => a.UpdatedOn)
                    .Take(count)
                    .ToListAsync();
            }
        }

        // REMOVE THIS DUPLICATE INTERFACE - It should be in a separate file
    }
