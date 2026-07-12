using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Exceptions;
using TransferService.Domain.ValueObjects;

namespace TransferService.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Money Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Wallet() { }

        public static Wallet Create(string documentId, string name, decimal initialBalance = 0)
        {
            ValidateDocumentId(documentId);

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidTransferException("Name is mandatory.");

            var now = DateTime.UtcNow;
            return new Wallet
            {
                DocumentId = documentId,
                Name = name,
                Balance = Money.Create(initialBalance),
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public void Debit(decimal amount)
        {
            var money = Money.Create(amount);

            if (money.Amount <= 0)
                throw new InvalidTransferException("The transfer amount must be greater than zero.");
            if (Balance.IsLessThan(money))
            throw new InsufficientBalanceException(Id, Balance.Amount, money.Amount);

            Balance = Balance.Subtract(money);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Credit(decimal amount)
        {
            var money = Money.Create(amount);
            if (money.Amount <= 0)
                throw new InvalidTransferException("The transfer amount must be greater than zero.");

        Balance = Balance.Add(money);
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateDocumentId(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId))
                throw new InvalidTransferException("Document ID is mandatory.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(documentId, @"^\d{13}$"))
                throw new InvalidTransferException("Document ID must have 13 characters.");
        }

    }
}
