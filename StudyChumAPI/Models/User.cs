using System.ComponentModel.DataAnnotations;

namespace StudyChumAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        //public int Role { get; set; }
    }
}
