
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TicketSystem.API.DTOs;
using TicketSystem.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        //inject dbContext
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        //create ticket
        [HttpPost]
        public IActionResult CreateTicket(CreateTicketRequest request)
        {
            if (string.IsNullOrEmpty(request.Subject))
                return BadRequest("Subject required");

            var ticket = new Ticket
            {
                TicketNumber = "TCK-" + Guid.NewGuid().ToString().Substring(0, 6),
                Subject = request.Subject,
                Description = request.Description,
                Priority = request.Priority,
                Status = "Open",
                CreatedDate = DateTime.UtcNow,
                CreatedById = request.UserId
            };

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return Ok(ticket);
        }


        //get ticket list role based
        [HttpGet("{userId}")]
        public IActionResult GetTickets(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user.Role == "Admin")
                return Ok(_context.Tickets.ToList());

            return Ok(_context.Tickets
                .Where(x => x.CreatedById == userId)
                .ToList());
        }

        //Update ticket status
        [HttpPut("update-status")]
        public IActionResult UpdateStatus(UpdateStatusRequest request)
        {
            //Check if ticket exists
            var ticket = _context.Tickets
                .FirstOrDefault(x => x.Id == request.TicketId);

            if (ticket == null)
                return NotFound("Ticket not found.");

            // Prevent modification if Closed
            if (ticket.Status == "Closed")
                return BadRequest("Closed tickets cannot be modified.");

            //  3. Validate status transition
            if (ticket.Status == "Open" && request.NewStatus != "In Progress")
                return BadRequest("Ticket must move from Open to In Progress.");

            if (ticket.Status == "In Progress" && request.NewStatus != "Closed")
                return BadRequest("Ticket must move from In Progress to Closed.");

            // 4. Save status history
            var history = new Ticketstatushistory
            {
                TicketId = ticket.Id,
                OldStatus = ticket.Status,
                NewStatus = request.NewStatus,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Ticketstatushistories.Add(history);

            // 5. Update ticket status
            ticket.Status = request.NewStatus;

            _context.SaveChanges();

            return Ok("Status updated successfully.");
        }


        //Add Comment

        [HttpPost("add-comment")]
        public IActionResult AddComment(int ticketId, string comment, string user)
        {
            var newComment = new Ticketcomment
            {
                TicketId = ticketId,
                Comment = comment,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.Ticketcomments.Add(newComment);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("details/{ticketId}")]
        public IActionResult GetTicketDetails(int ticketId)
        {
            var ticket = _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
                return NotFound("Ticket not found.");

            // Get assigned admin username
            string assignedTo = null;

            if (ticket.AssignedToId != null)
            {
                assignedTo = _context.Users
                    .Where(u => u.Id == ticket.AssignedToId)
                    .Select(u => u.Username)
                    .FirstOrDefault();
            }

            // Get status history
            var history = _context.Ticketstatushistories
                .Where(h => h.TicketId == ticketId)
                .OrderBy(h => h.UpdatedDate)
                .Select(h => new StatusHistoryDto
                {
                    OldStatus = h.OldStatus,
                    NewStatus = h.NewStatus,
                    UpdatedDate = h.UpdatedDate,
                    UpdatedBy = h.UpdatedBy
                })
                .ToList();

            // Get comments
            var comments = _context.Ticketcomments
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CreatedDate)
                .Select(c => new CommentDto
                {
                    Comment = c.Comment,
                    CreatedDate = c.CreatedDate,
                    CreatedBy = c.CreatedBy
                })
                .ToList();

            var response = new TicketDetailsResponse
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedDate = ticket.CreatedDate,
                AssignedTo = assignedTo,
                History = history,
                Comments = comments
            };

            return Ok(response);
        }

        [HttpPut("assign")]
        public IActionResult AssignTicket(AssignTicketRequest request)
        {
            // 1️⃣ Check ticket exists
            var ticket = _context.Tickets
                .FirstOrDefault(t => t.Id == request.TicketId);

            if (ticket == null)
                return NotFound("Ticket not found.");

            // 2️⃣ Prevent assigning closed ticket
            if (ticket.Status == "Closed")
                return BadRequest("Closed ticket cannot be assigned.");

            // 3️⃣ Check admin exists
            var admin = _context.Users
                .FirstOrDefault(u => u.Id == request.AdminUserId && u.Role == "Admin");

            if (admin == null)
                return BadRequest("Admin user not found.");

            // 4️⃣ Assign ticket
            ticket.AssignedToId = request.AdminUserId;

            // OPTIONAL: Log assignment in history
            var history = new Ticketstatushistory
            {
                TicketId = ticket.Id,
                OldStatus = ticket.Status,
                NewStatus = ticket.Status, // status not changing
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Ticketstatushistories.Add(history);

            _context.SaveChanges();

            return Ok("Ticket assigned successfully.");
        }










    }
}
