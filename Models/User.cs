using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }// unique identifier for each user
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;// "lecturer","Programme Coordinator","Academic Manager"
        public string Password { get; set; } = string.Empty;
    }
}

