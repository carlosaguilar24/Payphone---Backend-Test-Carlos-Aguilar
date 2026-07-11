using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Domain.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(int walletId, decimal balance, decimal requested)
        : base($"La billetera {walletId} tiene saldo insuficiente. Disponible: {balance}, solicitado: {requested}")
        {
        }
    }
}
