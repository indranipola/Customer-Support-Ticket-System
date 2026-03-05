using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using TicketSystemDesktop.Models;
using TicketSystemDesktop.Services;

namespace TicketSystemDesktop.Views
{
    public partial class TicketListView : Window
    {
        ApiService api = new ApiService();

        public TicketListView()
        {
            InitializeComponent();
            LoadTickets();
        }

        private async void LoadTickets()
        {
            var result = await api.GetAsync("tickets");

            var tickets = JsonConvert.DeserializeObject<List<Ticket>>(result);

            ticketGrid.ItemsSource = tickets;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTickets();
        }
    }
}
