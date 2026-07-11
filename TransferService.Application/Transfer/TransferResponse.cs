using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Application.Transfer
{
    public class TransferResponse
    {
        public bool Success { get; set; }   
        public string? Message { get; set; }

        private TransferResponse(bool  success, string? message)
        {
            Success = success;
            Message = message;
        }

        public static TransferResponse Ok() => new(true, null);
        public static TransferResponse Error(string message) => new(false, message);    
    }
}
