using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Data
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<Claim?> GetClaimByIdAsync(int id);
        Task<IEnumerable<Claim>> GetClaimsByStatusAsync(string status);
        Task<IEnumerable<Claim>> GetClaimsByLecturerAsync(int lecturerId);
        Task<IEnumerable<Claim>> GetClaimsByUserAsync(int userId);
        Task<Claim> AddClaimAsync(Claim claim);
        Task UpdateClaimAsync(Claim claim);
        Task DeleteClaimAsync(int id);
        Task<bool> ClaimExistsAsync(int id);
    }
}
