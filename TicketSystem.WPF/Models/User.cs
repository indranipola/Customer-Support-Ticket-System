using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.WPF.Models
{
    public class User
    {
        public int Id { get; set; }

        public string TicketNumber { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AssignedTo { get; set; }
    }
}
