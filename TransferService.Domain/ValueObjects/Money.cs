using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Exceptions;

namespace TransferService.Domain.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; }

        private Money(decimal amount)
        {
            Amount = amount;
        }

        public static Money Create(decimal amount)
        {
            if (amount < 0)
                throw new InvalidTransferException("Balance must be positive.");

            return new Money(amount);
        }

        public static Money Zero() => new(0);

        public Money Add(Money other) => new(Amount + other.Amount);


        public Money Subtract(Money other)
        {
            if (Amount < other.Amount)
                throw new InsufficientBalanceException(0, Amount, other.Amount);

            return new Money(Amount - other.Amount);
        }

        public bool IsGreaterThan(Money other) => Amount > other.Amount;
        public bool IsLessThan(Money other) => Amount < other.Amount;

        public bool Equals(Money? other) => other is not null && Amount == other.Amount;
        public override bool Equals(object? obj) => Equals(obj as Money);
        public override int GetHashCode() => Amount.GetHashCode();

        public static bool operator ==(Money? left, Money? right) => Equals(left, right);
        public static bool operator !=(Money? left, Money? right) => !Equals(left, right);
        public override string ToString() => Amount.ToString("F4");

    }
}
