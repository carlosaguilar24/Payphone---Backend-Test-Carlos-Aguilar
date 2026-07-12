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
        : base($"The wallet {walletId} has insuficent balance. Available: {balance}, requested: {requested}")
        {
        }
    }
}
