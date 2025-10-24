using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class AuthService
        {
            private readonly List<User> _users;

            public AuthService()
            {
                _users = new List<User>
            {
                new User {
                    UserId = 1,
                    Name = "Tshiamo Lentswe",
                    Email = "tshiamo@university.ac.za",
                    Password = "password",
                    Role = "Lecturer",
                    HourlyRate = 350.00m,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 2,
                    Name = "Prof. Sarah Johnson",
                    Email = "coordinator@university.ac.za",
                    Password = "password",
                    Role = "Coordinator",
                    HourlyRate = 0,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 3,
                    Name = "Dr. Michael Brown",
                    Email = "manager@university.ac.za",
                    Password = "password",
                    Role = "Manager",
                    HourlyRate = 0,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 4,
                    Name = "Admin User",
                    Email = "admin@university.ac.za",
                    Password = "password",
                    Role = "Admin",
                    HourlyRate = 0,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 5,
                    Name = "Ntokozo Nhleko",
                    Email = "ntokozo@university.ac.za",
                    Password = "password",
                    Role = "Lecturer",
                    HourlyRate = 320.00m,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 6,
                    Name = "Deshi Mfolo",
                    Email = "deshi@university.ac.za",
                    Password = "password",
                    Role = "Lecturer",
                    HourlyRate = 340.00m,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new User {
                    UserId = 7,
                    Name = "Kamogelo Lentswe",
                    Email = "kamogelo@university.ac.za",
                    Password = "password",
                    Role = "Lecturer",
                    HourlyRate = 330.00m,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            };
            }

            public User Authenticate(string email, string password)
            {
                return _users.FirstOrDefault(u =>
                    u.Email == email &&
                    u.Password == password &&
                    u.IsActive);
            }

            public User GetUserById(int userId)
            {
                return _users.FirstOrDefault(u => u.UserId == userId);
            }

            public List<User> GetAllUsers()
            {
                return _users.OrderBy(u => u.Name).ToList();
            }

            public List<User> GetUsersByRole(string role)
            {
                return _users.Where(u => u.Role == role && u.IsActive).ToList();
            }

            public bool ChangePassword(int userId, string oldPassword, string newPassword)
            {
                var user = _users.FirstOrDefault(u => u.UserId == userId);
                if (user != null && user.Password == oldPassword)
                {
                    user.Password = newPassword;
                    return true;
                }
                return false;
            }

            public void DeactivateUser(int userId)
            {
                var user = _users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.IsActive = false;
                }
            }

            public void ActivateUser(int userId)
            {
                var user = _users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.IsActive = true;
                }
            }
        }
    }