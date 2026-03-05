namespace TicketSystem.API.DTOs
{
    public class UpdateStatusRequest
    {
        public int TicketId { get; set; }
        public string NewStatus { get; set; }
        public string UpdatedBy { get; set; }

    }
}
