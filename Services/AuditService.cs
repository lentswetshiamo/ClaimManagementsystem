using ClaimManagementsystem.Data;
using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class AuditService
    {
        private readonly DatabaseContext _context;

        public AuditService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(int claimId, string action, string performedBy, string details)
        {
            var audit = new Audit
            {
                ClaimId = claimId,
                Action = action,
                PerformedBy = performedBy,
                Details = details,
                Timestamp = DateTime.Now
            };

            _context.Audits.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Audit>> GetClaimAuditHistoryAsync(int claimId)
        {
            return await Task.FromResult(_context.Audits
                .Where(a => a.ClaimId == claimId)
                .OrderByDescending(a => a.Timestamp)
                .ToList());
        }
    }
}
