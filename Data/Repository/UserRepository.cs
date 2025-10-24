using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Data.Repository
{
        public class UserRepository : IUserRepository
        {
            private readonly DatabaseContext _context;

            public UserRepository(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<User> GetByIdAsync(int userId)
            {
                return await _context.Users.FindAsync(userId);
            }

            public async Task<User> GetByEmailAsync(string email)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }

            public async Task<List<User>> GetAllAsync()
            {
                return await _context.Users.OrderBy(u => u.Name).ToListAsync();
            }

            public async Task<List<User>> GetByRoleAsync(string role)
            {
                return await _context.Users.Where(u => u.Role == role).ToListAsync();
            }

            public async Task<User> AddAsync(User user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<User> UpdateAsync(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<bool> DeleteAsync(int userId)
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            public async Task<bool> UserExistsAsync(string email)
            {
                return await _context.Users.AnyAsync(u => u.Email == email);
            }

            public async Task<List<User>> GetUsersByProgrammeAsync(int programmeId)
            {
                return await _context.Users
                    .Where(u => u.Role == "Lecturer")
                    .ToListAsync();
            }
        }
    }
