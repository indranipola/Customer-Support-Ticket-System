using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystemDesktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri("https://localhost:7076/api/");
        }

        // Return the full response so callers can inspect status codes and messages
        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, content);

            return response;
        }

        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
