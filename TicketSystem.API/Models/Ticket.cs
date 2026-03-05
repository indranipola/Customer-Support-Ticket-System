using System;
using System.Collections.Generic;

namespace TicketSystem.API.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string TicketNumber { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int CreatedById { get; set; }

    public int? AssignedToId { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<Ticketcomment> Ticketcomments { get; set; } = new List<Ticketcomment>();

    public virtual ICollection<Ticketstatushistory> Ticketstatushistories { get; set; } = new List<Ticketstatushistory>();
}
