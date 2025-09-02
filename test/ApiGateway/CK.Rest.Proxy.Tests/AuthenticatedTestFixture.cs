using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CK.Rest.Users.Shared.Forms;

namespace CK.Rest.Proxy.Tests
{
    public class AuthenticatedTestFixture : IDisposable
    {
        public HttpClient Client { get; }
        public string AdminToken { get; private set; }
        public uint AdminId { get; private set; }

        public AuthenticatedTestFixture()
        {
            Client = new HttpClient();
            SetupAdminUser().GetAwaiter().GetResult();
        }

        private async Task SetupAdminUser()
        {
            var content = new StringContent(JsonSerializer.Serialize(new UserCredentialsForm
            {
                Email = "admin@ck.com",
                Password = "admin"
            }), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{TestConfiguration.GetHost("Auth")}/Auth")
            {
                Content = content
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserResultForm>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            AdminToken = result.Token;
            AdminId = result.Id;
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
