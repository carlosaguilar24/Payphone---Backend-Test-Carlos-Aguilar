using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TransferService.Domain.Exceptions;
using TransferService.Domain.ValueObjects;
using Xunit;


namespace TransferService.Domain.Unittest.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Create_WithValidAmount_ShouldCreateInstance()
        {
            var money = Money.Create(100);

            money.Amount.Should().Be(100);
        }

        [Fact]
        public void Create_WithNegativeAmount_ShouldThrowException()
        {
            Action act = () => Money.Create(-10);

            act.Should().Throw<InvalidTransferException>();
        }

        [Fact]
        public void Add_ShouldSumCorrectly()
        {
            var a = Money.Create(100);
            var b = Money.Create(50);

            var result = a.Add(b);

            result.Amount.Should().Be(150);
        }


        [Fact]
        public void Subtract_WithSufficientBalance_ShouldSubtractCorrectly()
        {
            var a = Money.Create(100);
            var b = Money.Create(30);

            var result = a.Subtract(b);

            result.Amount.Should().Be(70);
        }

        [Fact]
        public void Subtract_WithInsufficientBalance_ShouldThrowException()
        {
            var a = Money.Create(50);
            var b = Money.Create(100);

            Action act = () => a.Subtract(b);

            act.Should().Throw<InsufficientBalanceException>();
        }

        [Fact]
        public void Equals_WithSameAmount_ShouldBeEqual()
        {
            var a = Money.Create(100);
            var b = Money.Create(100);

            (a == b).Should().BeTrue();
            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentAmount_ShouldNotBeEqual()
        {
            var a = Money.Create(100);
            var b = Money.Create(200);

            (a == b).Should().BeFalse();
        }
    }
}
