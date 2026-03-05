namespace TicketSystem.API.DTOs
{
    public class AssignTicketRequest
    {
        public int TicketId { get; set; }
        public int AdminUserId { get; set; }
        public string UpdatedBy { get; set; }

    }
}
