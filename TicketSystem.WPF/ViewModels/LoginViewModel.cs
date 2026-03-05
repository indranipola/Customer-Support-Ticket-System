using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using TicketSystem.WPF.Helpers;
using TicketSystem.WPF.Models;
using TicketSystem.WPF.Services;
using TicketSystem.WPF.Helpers;
using TicketSystem.WPF.Services;

namespace TicketSystem.WPF.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        ApiService api = new ApiService();

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private async void Login(object obj)
        {
            var data = new
            {
                Username = Username,
                Password = Password
            };

            var result = await api.Post("auth/login", data);

            if (result.Contains("Username"))
            {
                MessageBox.Show("Login Successful");
            }
            else
            {
                MessageBox.Show("Invalid Login");
            }
        }
    }
}
