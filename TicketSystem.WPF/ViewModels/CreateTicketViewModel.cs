using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketSystem.WPF.Helpers;

namespace TicketSystem.WPF.ViewModels
{
    public class CreateTicketViewModel
    {
        public string Subject { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }

    public ICommand CreateTicketCommand { get; }

    public CreateTicketViewModel()
    {
            CreateTicketCommand = new RelayCommand(param => CreateTicket());
    }

    private async void CreateTicket()
    {
        var ticket = new
        {
            Subject = Subject,
            Description = Description,
            Priority = Priority,
            Status = "Open"
        };

        var json = JsonSerializer.Serialize(ticket);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpClient client = new HttpClient())
        {
            await client.PostAsync("https://localhost:7076/api/tickets", content);
        }
    }

    }
}
