namespace ClaimManagementsystem.Data.Repository
{
      public interface IUserRepository
        {
            Task<User> GetByIdAsync(int userId);
            Task<User> GetByEmailAsync(string email);
            Task<List<User>> GetAllAsync();
            Task<List<User>> GetByRoleAsync(string role);
            Task<User> AddAsync(User user);
            Task<User> UpdateAsync(User user);
            Task<bool> DeleteAsync(int userId);
            Task<bool> UserExistsAsync(string email);
            Task<List<User>> GetUsersByProgrammeAsync(int programmeId);
        }
    }