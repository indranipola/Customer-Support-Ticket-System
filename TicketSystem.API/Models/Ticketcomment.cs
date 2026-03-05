using System;
using System.Collections.Generic;

namespace TicketSystem.API.Models;

public partial class Ticketcomment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
