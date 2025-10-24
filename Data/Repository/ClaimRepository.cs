using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Data.Repository
{
    public class ClaimRepository : IClaimRepository
        {
            private readonly DatabaseContext _context;

            public ClaimRepository(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<Claim> GetByIdAsync(int claimId)
            {
                return await _context.Claims
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.ClaimId == claimId);
            }

            public async Task<List<Claim>> GetByUserIdAsync(int userId)
            {
                return await _context.Claims
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.SubmittedDate)
                    .ToListAsync();
            }

            public async Task<List<Claim>> GetByStatusAsync(string status)
            {
                return await _context.Claims
                    .Include(c => c.User)
                    .Where(c => c.Status == status)
                    .OrderByDescending(c => c.SubmittedDate)
                    .ToListAsync();
            }

            public async Task<List<Claim>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
            {
                return await _context.Claims
                    .Include(c => c.User)
                    .Where(c => c.SubmittedDate >= startDate && c.SubmittedDate <= endDate)
                    .ToListAsync();
            }

            public async Task<Claim> AddAsync(Claim claim)
            {
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                return claim;
            }

            public async Task<Claim> UpdateAsync(Claim claim)
            {
                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();
                return claim;
            }

            public async Task<bool> DeleteAsync(int claimId)
            {
                var claim = await GetByIdAsync(claimId);
                if (claim != null)
                {
                    _context.Claims.Remove(claim);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            public async Task<List<Claim>> GetClaimsForReviewAsync(int coordinatorId)
            {
                return await _context.Claims
                    .Include(c => c.User)
                    .Where(c => c.Status == "Submitted")
                    .OrderBy(c => c.SubmittedDate)
                    .ToListAsync();
            }

            public async Task<List<Claim>> GetClaimsForFinalApprovalAsync()
            {
                return await _context.Claims
                    .Include(c => c.User)
                    .Where(c => c.Status == "Approved Level 1")
                    .OrderBy(c => c.SubmittedDate)
                    .ToListAsync();
            }

            public async Task<decimal> GetTotalApprovedAmountAsync(int userId, int year, int month)
            {
                return await _context.Claims
                    .Where(c => c.UserId == userId &&
                               c.Status.Contains("Approved") &&
                               c.Year == year)
                    .SumAsync(c => c.TotalAmount);
            }
        }

        public interface IClaimRepository
        {
            Task<Claim> GetByIdAsync(int claimId);
            Task<List<Claim>> GetByUserIdAsync(int userId);
            Task<List<Claim>> GetByStatusAsync(string status);
            Task<List<Claim>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
            Task<Claim> AddAsync(Claim claim);
            Task<Claim> UpdateAsync(Claim claim);
            Task<bool> DeleteAsync(int claimId);
            Task<List<Claim>> GetClaimsForReviewAsync(int coordinatorId);
            Task<List<Claim>> GetClaimsForFinalApprovalAsync();
            Task<decimal> GetTotalApprovedAmountAsync(int userId, int year, int month);
        }
    }
