using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using TicketSystemDesktop.Models;
using TicketSystemDesktop.Services;

namespace TicketSystemDesktop.Views
{
    public partial class LoginView : Window
    {
        private readonly ApiService api = new ApiService();

        public LoginView()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            // Client-side validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please enter a password.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            var loginData = new
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Password
            };

            // Fix: Use ApiService.PostAsync instead of accessing _client directly
            HttpResponseMessage response;
            try
            {
                response = await api.PostAsync("auth/login", loginData);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error: {ex.Message}", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // deserialize returned user
                User? user = null;
                try
                {
                    user = JsonConvert.DeserializeObject<User>(result);
                }
                catch (JsonException)
                {
                    // fall through to show an appropriate message
                }

                if (user != null)
                {
                    MessageBox.Show("Login Successful");

                    TicketListView dashboard = new TicketListView();
                    dashboard.Show();

                    this.Close();
                    return;
                }

                // If success status but deserialization failed, show server response for debugging
                MessageBox.Show("Unexpected server response.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Show server-provided message for failures (e.g., 400/401), fallback to ReasonPhrase
            var message = !string.IsNullOrWhiteSpace(result) ? result : response.ReasonPhrase ?? "Login failed.";
            MessageBox.Show(message, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}