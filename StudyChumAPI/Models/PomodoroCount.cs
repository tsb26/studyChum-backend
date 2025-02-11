using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyChumAPI.Models
{
    public class PomodoroCount
    {
        [Required]
        public int UserID { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int SessionCount { get; set; }
    }
}
