using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TransferService.Application.Wallets;

namespace TransferService.Integrationtest.Controller
{
    public class WalletsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient _client = null!;

        public WalletsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            _client = await AuthTestHelper.CreateAuthenticatedClientAsync(_factory);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task CreateWallet_WithValidData_ShouldReturn201AndCreatedWallet()
        {
            var request = new
            {
                documentId = "0801199912345",
                name = "Carlos Aguilar",
                initialBalance = 100
            };

            var response = await _client.PostAsJsonAsync("/api/v1/wallet", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var wallet = await response.Content.ReadFromJsonAsync<WalletResponse>();
            wallet!.DocumentId.Should().Be("0801199912345");
            wallet.Balance.Should().Be(100);
        }

        [Fact]
        public async Task CreateWallet_WithEmptyName_ShouldReturn400()
        {
            var request = new { documentId = "0801199912345", name = "", initialBalance = 100 };

            var response = await _client.PostAsJsonAsync("/api/v1/wallet", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateWallet_WithInvalidDocumentIdFormat_ShouldReturn400()
        {
            var request = new { documentId = "123", name = "Test User", initialBalance = 100 };

            var response = await _client.PostAsJsonAsync("/api/v1/wallet", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetWallet_WithNonExistentId_ShouldReturn404()
        {
            var response = await _client.GetAsync("/api/v1/wallet/999999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateWallet_ThenGetWallet_ShouldReturnSameData()
        {
            var createRequest = new { documentId = "0801199999999", name = "Test User", initialBalance = 50 };
            var createResponse = await _client.PostAsJsonAsync("/api/v1/wallet", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<WalletResponse>();

            var getResponse = await _client.GetAsync($"/api/v1/wallet/{created!.Id}");
            var fetched = await getResponse.Content.ReadFromJsonAsync<WalletResponse>();

            fetched!.Id.Should().Be(created.Id);
            fetched.Balance.Should().Be(50);
        }
    }
}
