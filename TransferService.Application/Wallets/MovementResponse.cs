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
        public int WalletId { get; set; }
        public decimal Amount { get;  set; }
        public string Type { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public Guid TransferId { get;  set; }
    }
}
