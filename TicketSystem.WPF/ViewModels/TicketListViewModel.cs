using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using TicketSystem.WPF.Models;
using TicketSystem.WPF.Services;

namespace TicketSystem.WPF.ViewModels
{
    public class TicketListViewModel : INotifyPropertyChanged
    {
        ApiService api = new ApiService();

        public ObservableCollection<Ticket> Tickets { get; set; }

        public TicketListViewModel()
        {
            LoadTickets();
        }

        private async void LoadTickets()
        {
            var json = await api.Get("tickets");
            var tickets = JsonSerializer.Deserialize<List<Ticket>>(json);

            Tickets = new ObservableCollection<Ticket>(tickets);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tickets)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
