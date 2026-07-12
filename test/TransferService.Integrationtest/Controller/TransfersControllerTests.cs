using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TransferService.Application.Wallets;

namespace TransferService.Integrationtest.Controller
{
    public class TransfersControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient _client = null!;

        public TransfersControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            _client = await AuthTestHelper.CreateAuthenticatedClientAsync(_factory);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ExecuteTransfer_WithValidWallets_ShouldDebitAndCreditInDatabase()
        {
            var origin = await CreateWalletAsync("0801199911111", "Origin", 100);
            var destination = await CreateWalletAsync("0801199922222", "Destination", 0);

            var transferRequest = new
            {
                transferId = Guid.NewGuid(),
                fromWalletId = origin.Id,
                toWalletId = destination.Id,
                amount = 40
            };

            var response = await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var originAfter = await GetWalletAsync(origin.Id);
            var destinationAfter = await GetWalletAsync(destination.Id);

            originAfter.Balance.Should().Be(60);
            destinationAfter.Balance.Should().Be(40);
        }

        [Fact]
        public async Task ExecuteTransfer_WithInsufficientBalance_ShouldReturn422AndNotChangeBalances()
        {
            var origin = await CreateWalletAsync("0801199933333", "Origin", 10);
            var destination = await CreateWalletAsync("0801199944444", "Destination", 0);

            var transferRequest = new
            {
                transferId = Guid.NewGuid(),
                fromWalletId = origin.Id,
                toWalletId = destination.Id,
                amount = 100
            };

            var response = await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var originAfter = await GetWalletAsync(origin.Id);
            originAfter.Balance.Should().Be(10); // unchanged, transaction rolled back
        }

        [Fact]
        public async Task ExecuteTransfer_WithNonExistentDestinationWallet_ShouldReturn404()
        {
            var origin = await CreateWalletAsync("0801199977777", "Origin", 100);

            var transferRequest = new
            {
                transferId = Guid.NewGuid(),
                fromWalletId = origin.Id,
                toWalletId = 999999,
                amount = 10
            };

            var response = await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ExecuteTransfer_WithSameTransferId_ShouldBeIdempotent()
        {
            var origin = await CreateWalletAsync("0801199955555", "Origin", 100);
            var destination = await CreateWalletAsync("0801199966666", "Destination", 0);
            var transferId = Guid.NewGuid();

            var transferRequest = new
            {
                transferId,
                fromWalletId = origin.Id,
                toWalletId = destination.Id,
                amount = 30
            };

            // Execute the SAME transfer twice
            await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);
            await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);

            var originAfter = await GetWalletAsync(origin.Id);

            // Balance should reflect only ONE debit, not two
            originAfter.Balance.Should().Be(70);
        }

        [Fact]
        public async Task ExecuteTransfer_WithSameOriginAndDestination_ShouldReturn400()
        {
            var origin = await CreateWalletAsync("0801199988888", "Origin", 100);

            var transferRequest = new
            {
                transferId = Guid.NewGuid(),
                fromWalletId = origin.Id,
                toWalletId = origin.Id,
                amount = 10
            };

            var response = await _client.PostAsJsonAsync("/api/v1/transfer", transferRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task<WalletResponse> CreateWalletAsync(string documentId, string name, decimal balance)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/wallet",
                new { documentId, name, initialBalance = balance });
            return (await response.Content.ReadFromJsonAsync<WalletResponse>())!;
        }

        private async Task<WalletResponse> GetWalletAsync(int id)
        {
            var response = await _client.GetAsync($"/api/v1/wallet/{id}");
            return (await response.Content.ReadFromJsonAsync<WalletResponse>())!;
        }
    }
}
