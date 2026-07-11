using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Application.Wallet
{
    public class CreateWalletRequest
    {
        public string DocumentId {  get; set; }
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }

    }
}
