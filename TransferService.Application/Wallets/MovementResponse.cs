using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Enums;

namespace TransferService.Application.Wallets
{
    public class MovementResponse
    {
        public int WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public MovementType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid TransferId { get; private set; }
    }
}
