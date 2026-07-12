using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using TransferService.Application.Interfaces;
using TransferService.Application.Transfer;
using TransferService.Domain.Entities;
using TransferService.Domain.Enums;
using TransferService.Domain.Exceptions;

namespace TransferService.Application.Unittest.Transfers
{
    public class WalletTransferServiceTests
    {
        private readonly IWalletRepository _walletRepository = Substitute.For<IWalletRepository>();
        private readonly IMovementRepository _movementRepository = Substitute.For<IMovementRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly WalletTransferService _sut;

        public WalletTransferServiceTests()
        {
            _sut = new WalletTransferService(_walletRepository, _movementRepository, _unitOfWork);
        }

        [Fact]
        public async Task ExecuteTransferAsync_WithValidWallets_ShouldDebitAndCreditCorrectly()
        {
            var origin = Wallet.Create("0801199912345", "Origin", 100);
            var destination = Wallet.Create("0801199954321", "Destination", 50);

            _movementRepository.GetMovementByIdTransactionAsync(Arg.Any<Guid>())
                .Returns(new List<Movement>());
            _walletRepository.GetWalletByIdAsync(1).Returns(origin);
            _walletRepository.GetWalletByIdAsync(2).Returns(destination);

            var request = new TransferRequest
            {
                TransferId = Guid.NewGuid(),
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 30
            };

            var result = await _sut.ExecuteTransferAsync(request);

            origin.Balance.Amount.Should().Be(70);
            destination.Balance.Amount.Should().Be(80);
            result.Amount.Should().Be(30);

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ExecuteTransferAsync_WithInsufficientBalance_ShouldThrowAndNotSave()
        {
            var origin = Wallet.Create("0801199912345", "Origin", 10);
            var destination = Wallet.Create("0801199954321", "Destination", 50);

            _movementRepository.GetMovementByIdTransactionAsync(Arg.Any<Guid>())
                .Returns(new List<Movement>());
            _walletRepository.GetWalletByIdAsync(1).Returns(origin);
            _walletRepository.GetWalletByIdAsync(2).Returns(destination);

            var request = new TransferRequest
            {
                TransferId = Guid.NewGuid(),
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 100
            };

            Func<Task> act = () => _sut.ExecuteTransferAsync(request);

            await act.Should().ThrowAsync<InsufficientBalanceException>();

            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ExecuteTransferAsync_WithNonExistentOriginWallet_ShouldThrowException()
        {
            _movementRepository.GetMovementByIdTransactionAsync(Arg.Any<Guid>())
                .Returns(new List<Movement>());
            _walletRepository.GetWalletByIdAsync(999).Returns((Wallet?)null);

            var request = new TransferRequest
            {
                TransferId = Guid.NewGuid(),
                FromWalletId = 999,
                ToWalletId = 2,
                Amount = 50
            };

            Func<Task> act = () => _sut.ExecuteTransferAsync(request);

            await act.Should().ThrowAsync<WalletNotFoundException>();
        }

        [Fact]
        public async Task ExecuteTransferAsync_WithSameOriginAndDestinationWallet_ShouldThrowException()
        {
            var request = new TransferRequest
            {
                TransferId = Guid.NewGuid(),
                FromWalletId = 1,
                ToWalletId = 1,
                Amount = 50
            };

            Func<Task> act = () => _sut.ExecuteTransferAsync(request);

            await act.Should().ThrowAsync<InvalidTransferException>();
        }

        [Fact]
        public async Task ExecuteTransferAsync_WithAlreadyProcessedTransferId_ShouldReturnExistingResultWithoutReprocessing()
        {
            var transferId = Guid.NewGuid();
            var existingMovements = new List<Movement>
            {
                Movement.CreateMovement(1, 50, MovementType.Debit, transferId),
                Movement.CreateMovement(2, 50, MovementType.Credit, transferId)
            };

            _movementRepository.GetMovementByIdTransactionAsync(transferId)
                .Returns(existingMovements);

            var request = new TransferRequest
            {
                TransferId = transferId,
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 50
            };

            var result = await _sut.ExecuteTransferAsync(request);

            result.TransferId.Should().Be(transferId);

            await _walletRepository.DidNotReceive().GetWalletByIdAsync(Arg.Any<int>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
