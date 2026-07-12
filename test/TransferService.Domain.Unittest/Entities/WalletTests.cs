using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TransferService.Domain.Entities;
using TransferService.Domain.Exceptions;
using Xunit;


namespace TransferService.Domain.Unittest.Entities
{
    public class WalletTests
    {
        private const string ValidDocumentId = "0801199912345";
        private const string ValidName = "Carlos Aguilar";

        [Fact]
        public void Create_WithValidData_ShouldCreateWalletCorrectly()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            wallet.DocumentId.Should().Be(ValidDocumentId);
            wallet.Name.Should().Be(ValidName);
            wallet.Balance.Amount.Should().Be(100);
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowException()
        {
            Action act = () => Wallet.Create(ValidDocumentId, "", 100);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Create_WithInvalidDocumentIdFormat_ShouldThrowException()
        {
            Action act = () => Wallet.Create("123", ValidName, 100);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Create_WithNegativeInitialBalance_ShouldThrowException()
        {
            Action act = () => Wallet.Create(ValidDocumentId, ValidName, -50);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Debit_WithSufficientBalance_ShouldReduceBalance()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            wallet.Debit(30);

            wallet.Balance.Amount.Should().Be(70);
        }

        [Fact]
        public void Debit_WithInsufficientBalance_ShouldThrowException()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 50);

            Action act = () => wallet.Debit(100);

            act.Should().Throw<InsufficientBalanceException>();
        }

        [Fact]
        public void Debit_WithZeroAmount_ShouldThrowException()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            Action act = () => wallet.Debit(0);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Debit_WithNegativeAmount_ShouldThrowException()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            Action act = () => wallet.Debit(-10);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Credit_ShouldIncreaseBalance()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            wallet.Credit(50);

            wallet.Balance.Amount.Should().Be(150);
        }

        [Fact]
        public void Credit_WithZeroAmount_ShouldThrowException()
        {
            var wallet = Wallet.Create(ValidDocumentId, ValidName, 100);

            Action act = () => wallet.Credit(0);

            act.Should().Throw<InvalidTransferException>();
        }

    }
}
