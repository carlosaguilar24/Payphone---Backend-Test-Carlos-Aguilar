using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Integrationtest
{
    public static class AuthTestHelper
    {
        public static async Task<HttpClient> CreateAuthenticatedClientAsync(CustomWebApplicationFactory factory)
        {
            var client = factory.CreateClient();

            var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", new
            {
                username = "payphone",
                password = "payphonepassword"
            });

            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult!.Token);

            return client;
        }

        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
