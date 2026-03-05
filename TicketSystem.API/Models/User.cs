using System;
using System.Collections.Generic;

namespace TicketSystem.API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Ticket> TicketAssignedTos { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketCreatedBies { get; set; } = new List<Ticket>();
}
