using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.WPF.Services
{
    public class ApiService
    {
        HttpClient client = new HttpClient();

        string baseUrl = "https://localhost:7076/api/";

        public async Task<string> Post(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl + url, content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string url)
        {
            var response = await client.GetAsync(baseUrl + url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
