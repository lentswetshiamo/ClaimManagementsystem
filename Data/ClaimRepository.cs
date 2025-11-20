using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Data
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly DatabaseContext _context;

        public ClaimRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            return await _context.Claims.OrderByDescending(c => c.DateSubmitted).ToListAsync();
        }

        public async Task<Claim?> GetClaimByIdAsync(int id)
        {
            return await _context.Claims.FindAsync(id);
        }

        public async Task<IEnumerable<Claim>> GetClaimsByStatusAsync(string status)
        {
            return await _context.Claims
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsByLecturerAsync(int lecturerId)
        {
            return await _context.Claims
                .Where(c => c.LecturerId == lecturerId)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsByUserAsync(int userId)
        {
            return await _context.Claims
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();
        }

        public async Task<Claim> AddClaimAsync(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task UpdateClaimAsync(Claim claim)
        {
            _context.Entry(claim).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClaimAsync(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClaimExistsAsync(int id)
        {
            return await _context.Claims.AnyAsync(c => c.Id == id);
        }
    }
}
