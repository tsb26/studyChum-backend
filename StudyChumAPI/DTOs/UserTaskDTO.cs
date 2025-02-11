namespace StudyChumAPI.DTOs
{
    public class UserTaskDTO
    {
        public int TaskID { get; set; }
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime Date { get; set; }
    }
}
