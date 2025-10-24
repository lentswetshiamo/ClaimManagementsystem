using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Data.Repository
{
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
        Task AddAsync(Claim claim);
    }
    }