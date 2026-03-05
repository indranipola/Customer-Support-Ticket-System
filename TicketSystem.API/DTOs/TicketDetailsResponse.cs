namespace TicketSystem.API.DTOs
{
    public class TicketDetailsResponse
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AssignedTo { get; set; }

        public List<StatusHistoryDto> History { get; set; }
        public List<CommentDto> Comments { get; set; }

    }

    public class StatusHistoryDto
    {
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class CommentDto
    {
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
