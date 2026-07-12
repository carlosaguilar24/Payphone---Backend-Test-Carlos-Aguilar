using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TransferService.Application.Wallets;

namespace TransferService.Integrationtest.Controller
{
    public class MovementsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient _client = null!;

        public MovementsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            _client = await AuthTestHelper.CreateAuthenticatedClientAsync(_factory);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetMovements_AfterTransfer_ShouldReturnDebitAndCreditMovements()
        {
            var origin = await CreateWalletAsync("0801199900001", "Origin", 100);
            var destination = await CreateWalletAsync("0801199900002", "Destination", 0);

            await _client.PostAsJsonAsync("/api/v1/transfer", new
            {
                transferId = Guid.NewGuid(),
                fromWalletId = origin.Id,
                toWalletId = destination.Id,
                amount = 25
            });

            var response = await _client.GetAsync($"/api/v1/wallet/{origin.Id}/movements");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("25");
        }

        [Fact]
        public async Task GetMovements_ForWalletWithoutMovements_ShouldReturnEmptyList()
        {
            var wallet = await CreateWalletAsync("0801199900099", "No Movements", 50);

            var response = await _client.GetAsync($"/api/v1/wallet/{wallet.Id}/movements");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMovements_ForNonExistentWallet_ShouldReturn404()
        {
            var response = await _client.GetAsync("/api/v1/wallet/99999/movements");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<WalletResponse> CreateWalletAsync(string documentId, string name, decimal balance)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/wallet",
                new { documentId, name, initialBalance = balance });
            return (await response.Content.ReadFromJsonAsync<WalletResponse>())!;
        }
        
    }
}
