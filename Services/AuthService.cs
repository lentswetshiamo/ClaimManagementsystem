using ClaimManagementsystem.Data;

namespace ClaimManagementsystem.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Models.User?> ValidateUserAsync(string email, string password)
        {
            return await _userRepository.ValidateUserAsync(email, password);
        }

        public async Task<Models.User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<Models.User> RegisterUserAsync(Models.User user)
        {
            // In production, hash the password before storing
            return await _userRepository.AddUserAsync(user);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.UserExistsAsync(email);
        }

        public bool IsUserInRole(Models.User user, string role)
        {
            return user.Role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsUserInAnyRole(Models.User user, params string[] roles)
        {
            return roles.Any(role => user.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
        }
    }
}
