using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyChumAPI.Models
{
    public class UserTask
    {
        [Key]
        public int TaskID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
