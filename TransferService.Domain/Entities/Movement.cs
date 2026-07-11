using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Enums;

namespace TransferService.Domain.Entities
{
    public class Movement
    {
        public int Id { get; private set; }
        public int WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public MovementType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid TransferId { get; private set; }

        public Movement()
        {
        }

        public static Movement CreateMovement(int walletId, decimal amount, MovementType type, Guid transferId) => new ()
        {
            WalletId = walletId,
            Amount = amount,
            CreatedAt = DateTime.UtcNow,
            TransferId = transferId,
            Type = type
        };

    }
}
