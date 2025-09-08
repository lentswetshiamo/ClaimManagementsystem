using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
    public class Lecturer
    {
        [Required]
        public int Id { get; set; }
        public string Topic { get; set; } = string.Empty;// topic of the lecture
        public string LecturerName { get; set; } = string.Empty;// name of the lecturer
        public System.DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;// brief description of the lecture
    }
}
