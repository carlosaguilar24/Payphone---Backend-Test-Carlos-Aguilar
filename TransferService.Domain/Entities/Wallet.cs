using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Exceptions;

namespace TransferService.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Wallet() { }

        public static Wallet Create(string documentId, string name, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(documentId))
                throw new ArgumentException("El documento de identidad es obligatorio.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio.");
            if (initialBalance < 0)
                throw new InvalidTransferException("El saldo inicial no puede ser negativo.");

            var now = DateTime.UtcNow;
            return new Wallet
            {
                DocumentId = documentId,
                Name = name,
                Balance = initialBalance,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public void Debit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidTransferException("El monto a debitar debe ser mayor a cero.");
            if (Balance < amount)
                throw new InsufficientBalanceException(Id, Balance, amount);

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidTransferException("El monto a acreditar debe ser mayor a cero.");

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
