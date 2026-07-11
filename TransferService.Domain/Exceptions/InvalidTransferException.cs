using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Domain.Exceptions
{
    public class InvalidTransferException : Exception
    {
        public InvalidTransferException(string message) : base(message)
        {
        }
    }
}
